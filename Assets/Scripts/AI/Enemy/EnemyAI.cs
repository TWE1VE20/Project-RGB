using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class EnemyAI : MonoBehaviour, IDamagable
{
    public enum State { Idle, Trace, Patrol, PatrolIdle, Groggy, Alert, Return, Battle, Die, Gameover }

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
    [SerializeField] public Transform firstTarget;
    [SerializeField] protected Vector3 lostPosition;

    [Header("FindTarget")]
    private float preAngle;
    private float cosAngle;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float cosRange;
    [SerializeField] protected Collider[] atkColliders = new Collider[20];
    [SerializeField] protected Vector3 moveDir;
    [SerializeField] protected Vector3 myPos;
    [SerializeField] 

    [Header("Alert")]
    protected float Delay;
    protected bool alertArrive;
    [SerializeField] protected float findDelay;
    [SerializeField] protected Vector3 findpostion;

    [Header("SnipeLine")]
    [SerializeField] protected PlayerDetecter2[] playerDetecter2s;
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
    [SerializeField] protected bool arrive;
    [SerializeField] protected float idleTime;

    [Header("Speed")]
    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float TraceSpeed;
    [SerializeField] protected float BattleSpeed;
    [SerializeField] protected float ReturnSpeed;

    [Header("Color")]
    [SerializeField] protected HaveColor haveColor;
    [SerializeField] protected HaveColor.ThisColor InitColor;
    [SerializeField] protected Renderer[] renders;
    [SerializeField] protected Material curColor;

    [Header("Security")]
    [SerializeField] protected HeadBanging headBanging;
    [SerializeField] private float securityRotationSpeed = 1.0f; // 회전 속도
    [SerializeField] private float rotationAngle = 30.0f; // 회전 각도 (도)
    [SerializeField] private float rotationWait = 3.0f; // 회전 끝나고 대기시간
    private bool isRotating = false; // 회전 중인지 여부
    private float currentRotationAngle = 0.0f; // 현재 회전 각도


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


    // 배틀모드 돌입시  firstTarget 과 attackPoint를 별개로 저장. firstTarget이 null이 될 시 attackPoint로 공격. firstTarget이 null 이고 attack가 0이면 alert모드로 전환
    // alert 모드로 전환해서 attackPoint.position 으로 이동하는 스크립트가서 없으면 return 모드로 전환 예정
    
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
                    lostPosition = atkColliders[i].transform.position;
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
        if (attackCost >= attackCooltime)
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
    } // 적 공격(시야 내)
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
    public void search()
    {
        if ( Vector3.Distance(transform.position, lostPosition) < 2f)
        {
            animator.SetBool("Walk", false);
            alertArrive = true;
        }
    } // 적 마지막 목격위치로 이동

    private float curSpeed;
    public void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            patrolTarget = patrolTarget == patrolPosition1 ? patrolPosition2 : patrolPosition1;
            arrive = true;
        }
    } // 적 순찰 목표
    public IEnumerator PatrolIdle()
    {
        yield return new WaitForSeconds(idleTime);
        arrive = false;
    } // 순찰 대기 시간
    public void Direction()
    {
        if (firstTarget != null)
        {
            // x축 기준 타겟 방향 벡터 계산
            Vector3 targetDirection = new Vector3(firstTarget.position.x - transform.position.x, 0.0f, firstTarget.position.z - transform.position.z);

            // 쿼터니언 회전값 계산 (x축만 회전)
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // 부드러운 회전 (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // x축 기준 사라진 위치 방향 벡터 계산
            Vector3 targetDirection = new Vector3(lostPosition.x - transform.position.x, 0.0f, lostPosition.z - transform.position.z);

            // 쿼터니언 회전값 계산 (x축만 회전)
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // 부드러운 회전 (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    } // 적 바라보는 방향

    public void Directionex()// 방향 회전 구버전
    {
        if (firstTarget != null)
        {
            //transform.LookAt(firstTarget);
            Quaternion targetRotation = Quaternion.LookRotation(firstTarget.position - viewPoint.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(lostPosition - viewPoint.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            return;
        }
        
    }
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
    public void Laser()
    {
        playerDetecter2s = GetComponentsInChildren<PlayerDetecter2>();
        foreach (PlayerDetecter2 playerDetecter2 in playerDetecter2s)
        {
            playerDetecter2.targetting = true;
        }
    } // 경고선 레이저로
    public void ColorChange()
    {
        renders = GetComponentsInChildren<MeshRenderer>();
        

        foreach (Renderer render in renders)
        {
            foreach (Material material in render.materials)
            {
                material.color = haveColor.MaterialColor();
            }
        }
        
    }// 색 반영 랜더러 가져오는 버전
    public void ColorChanger()
    {
        curColor.color = haveColor.MaterialColor();
        // 부모 오브젝트 가져오기
        GameObject parentObject = this.gameObject; // 실제 부모 오브젝트로 교체

        // 자식 오브젝트 반복 처리
        foreach (Transform child in parentObject.transform)
        {
            // 자식 오브젝트의 Renderer 컴포넌트 접근
            Renderer renderer = child.GetComponent<Renderer>();

            // Renderer가 존재하면 색깔 설정
            if (renderer != null)
            {
                renderer.material.color = curColor.color;
            }
        }
    }// 마테리얼 가져오는 버전
    public void Security()
    {
        if (isRotating == false)
        {
            StartCoroutine(RotateObject());
        }
        
    }
    private IEnumerator RotateObject()
    {
        isRotating = true;

        // 회전 방향 설정
        bool isForward = true;

        while (true)
        {
            // 현재 바라보고 있는 방향 계산
            Vector3 forward = transform.forward;
            forward.y = 0.0f; // Y축 방향 제외

            // 현재 회전 각도 계산
            float targetRotationAngle;
            if (isForward)
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) + rotationAngle;
            }
            else
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) - rotationAngle;
            }

            // Lerp 함수를 사용하여 회전 보간
            while (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.1f)
            {
                currentRotationAngle = Mathf.Lerp(currentRotationAngle, targetRotationAngle, securityRotationSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
                yield return null;
            }

            // 회전 방향 반전
            isForward = !isForward;

            // 잠시 대기
            yield return new WaitForSeconds(rotationWait);
            //isRotating = false;
        }
    }
} 

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

