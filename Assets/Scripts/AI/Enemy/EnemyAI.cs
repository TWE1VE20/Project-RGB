using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class EnemyAI : MonoBehaviour, IDamagable/*, IStunable*/
{
    public enum State { Idle, Trace, Patrol, Groggy, Avoid, Return, Battle, Die, Gameover }

    [Header("Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] protected Rigidbody rigid;

    [Header("Attack")]
    [SerializeField] protected bool debug;
    [SerializeField] protected LayerMask targetLayerMask;
    [SerializeField] protected float attackRange;
    [SerializeField] protected int deal;
    [SerializeField] protected float attackCost;
    [SerializeField] protected float attackCooltime;
    [SerializeField] protected Transform firstTarget;
    [SerializeField] protected Transform secondTarget;

    [Header("FindTarget")]
    private float preAngle;
    private float cosAngle;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float cosRange;
    [SerializeField] protected Collider[] atkColliders = new Collider[20];
    [SerializeField] protected Vector3 moveDir;
    [SerializeField] protected Vector3 myPos;

    [Header("SnipeLine")]
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected float showDuration;
    [SerializeField] protected float hideDuration;
    [SerializeField] protected float timer;
    [SerializeField] protected bool isVisible;
    

    [Header("Spec")]
    [SerializeField] protected int MaxHP;
    [SerializeField] protected int hp;
    [SerializeField] protected bool isDied;
    [SerializeField] protected Transform viewPoint;
    [SerializeField] protected LayerMask obstacleLayerMask;
    [SerializeField] protected float addTargetRange;
    [SerializeField] protected float traceRange;
    [SerializeField] protected float avoidRange;
    [SerializeField] protected bool groggyAble;


    [Header("gizmo")]
    [SerializeField, Range(0, 360)] public float angle;

    [Header("Move")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("Patrol")]
    [SerializeField] protected GameObject patrolPointObject1;
    [SerializeField] protected GameObject patrolPointObject2;
    [SerializeField] public Vector3 patrolPosition1;
    [SerializeField] public Vector3 patrolPosition2;
    [SerializeField] protected Vector3 patrolTarget;
    [SerializeField] protected Vector3 returnPoint;

    [Header("Speed")]
    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float TraceSpeed;
    [SerializeField] protected float BattleSpeed;
    [SerializeField] protected float ReturnSpeed;

    [Header("Color")]
    [SerializeField] protected HaveColor haveColor;
    [SerializeField] protected HaveColor.ThisColor InitColor;


    
    public float CosAngle
    {
        get
        {
            if (preAngle == angle)
                return cosAngle;

            preAngle = angle;
            cosAngle = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
            return cosAngle;
        }
    }

    protected void Awakefirst()
    {
        haveColor = gameObject.AddComponent<HaveColor>();
        haveColor.SetColor(InitColor);
    }


    public void TakeDamage(int damage)
    {
        Debug.Log("damaged");
        hp -= damage;
        if (hp <= 0)
        {
            //battleAni.Play()
        }
    }

    public void TakeDamage(int damage, Vector3 EnemyPosition)
    {
        return;
    }

    public void TakeDamage(Weapons.Colors color)
    {
        haveColor.PeelColor(color);
        return;
    }


    

    public void FindTarget() // 적 탐색 하는 부분
    {
        if (firstTarget == null)
        {
            int size = Physics.OverlapSphereNonAlloc(viewPoint.position, addTargetRange, atkColliders, targetLayerMask);
            for (int i = 0; i < size; i++)
            {
                {
                    Vector3 dirToTarget = (atkColliders[i].transform.position - viewPoint.position).normalized;

                    if (Vector3.Dot(viewPoint.forward, dirToTarget) < CosAngle)
                        continue;

                    float distToTarget = Vector3.Distance(atkColliders[i].transform.position, viewPoint.position);
                    if (Physics.Raycast(viewPoint.position, dirToTarget, distToTarget, obstacleLayerMask))
                        continue;

                    Debug.DrawRay(viewPoint.position, dirToTarget * distToTarget, Color.red);
                    firstTarget = atkColliders[i].transform;

                    moveDir = dirToTarget;
                    return;
                }
            }
        }
        else if (firstTarget != null)
        {
            float distToTarget = Vector3.Distance(firstTarget.transform.position, viewPoint.position);
            if (distToTarget > traceRange)
            {
                firstTarget = null;
            }
        }

    }
    public void Attack()
    {
        attackCost += Time.deltaTime;
        if (attackCost >= 3f)
        {
            //owner.StopCoroutine(AttackCoroutine());
            //RaycastHit hit; 레이 발사
            if (Physics.Raycast(viewPoint.position, viewPoint.forward, out RaycastHit hit, attackRange, targetLayerMask))
            {
                // 레이가 IDamagable 인터페이스를 구현한 오브젝트에 충돌했다면
                IDamagable damageable = hit.collider.gameObject.GetComponent<IDamagable>();
                // TakeDamage 함수를 호출하여 피해를 입힙니다.
                Debug.Log(hit.collider.gameObject.name);
                damageable?.TakeDamage(deal, transform.position);
                attackCost = 0;
            }
            Debug.Log("Attacking");
            //owner.attackCost--;
            //owner.StartCoroutine(AttackCoroutine());
        }
    } // 적 공격
    public void Move()
    {
        if (firstTarget != null)
        {
            Debug.Log("moving");
            agent.destination = firstTarget.transform.position;
        }
        if (firstTarget == null)
        {
            agent.destination = patrolTarget;
            return;
        }
    } // 적 이동
    public void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            patrolTarget = patrolTarget == patrolPosition1 ? patrolPosition2 : patrolPosition1;
        }
    } // 적 순찰 목표
    public void Direction()
    {
        if (firstTarget != null)
        {
            //transform.LookAt(firstTarget);
            Quaternion targetRotation = Quaternion.LookRotation(firstTarget.position - viewPoint.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            return;
        }

    } // 적 바라보는 방향
    public void Line()
    {
        if (firstTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2; // 두 개의 정점으로 선을 만듭니다.
            lineRenderer.SetPosition(0, viewPoint.transform.position);
            lineRenderer.SetPosition(1, firstTarget.transform.position);
            timer += Time.deltaTime;
            if (timer >= (isVisible ? showDuration : hideDuration))
            {
                timer = 0f;
                isVisible = !isVisible;

                //lineRenderer 활성화, 비활성화 토글
                lineRenderer.enabled = isVisible;
            }


        }
        else
        {
            lineRenderer.enabled = false;
        }
    } // 경고선

    //public void ListChoice()
    //{
    //    if (gameObject.layer == 8)
    //    {
    //        this.enemyList = Manager.Battle.redAI;
    //        layerMask = 512;
    //        gravePos = new Vector3(-12.9f, 5.74f, 0);
    //    }
    //    else if (gameObject.layer == 9)
    //    {
    //        this.enemyList = Manager.Battle.blueAI;
    //        layerMask = 256;
    //        gravePos = new Vector3(12.9f, -5.74f, 0);
    //    }
    //} // 적 리스트에서 탐색시 리스트 선택
}
