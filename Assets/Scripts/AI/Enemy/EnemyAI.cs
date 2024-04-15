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
    [SerializeField] protected float attackRange = 8f;
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
    [SerializeField] protected float addTargetRange = 6f;
    [SerializeField] protected float traceRange = 10f;
    [SerializeField] protected float battleRange = 8f;
    [SerializeField] protected float patrolRange = 8f;
    [SerializeField] protected float idleRange = 6f;
    [SerializeField] protected float alertRange = 8f;
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
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float TraceSpeed = 3f;
    [SerializeField] protected float ReturnSpeed = 5f;

    [Header("Color")]
    [SerializeField] protected HaveColor haveColor;
    [SerializeField] protected HaveColor.ThisColor InitColor;
    [SerializeField] protected Renderer[] renders;
    [SerializeField] protected Material curColor;
    [SerializeField] protected Collider[] deathCollider;

    [Header("Security")]
    [SerializeField] protected HeadBanging headBanging;
    [SerializeField] private float securityRotationSpeed = 1.0f; // ȸ�� �ӵ�
    [SerializeField] private float rotationAngle = 30.0f; // ȸ�� ���� (��)
    [SerializeField] private float rotationWait = 3.0f; // ȸ�� ������ ���ð�
    private bool isRotating = false; // ȸ�� ������ ����
    private float currentRotationAngle = 0.0f; // ���� ȸ�� ����
    public Quaternion initialLocalRotation;

    [Header("Dead")]
    [SerializeField] protected float deadDelay;
    [SerializeField] protected Collider[] regCols;
    [SerializeField] protected Rigidbody[] regRigids;
    [SerializeField] protected CharacterJoint[] regJoints;

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


    // ��Ʋ��� ���Խ�  firstTarget �� attackPoint�� ������ ����. firstTarget�� null�� �� �� attackPoint�� ����. firstTarget�� null �̰� attack�� 0�̸� alert���� ��ȯ
    // alert ���� ��ȯ�ؼ� attackPoint.position ���� �̵��ϴ� ��ũ��Ʈ���� ������ return ���� ��ȯ ����
    
    public void FindTarget() // �� Ž�� �ϴ� �κ�
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
            Vector3 dirToTarget = (firstTarget.transform.position - viewPoint.position).normalized;
            if (Physics.Raycast(viewPoint.position, dirToTarget, distToTarget, obstacleLayerMask) || distToTarget > traceRange)
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
            //RaycastHit hit; ���� �߻�
            if (Physics.Raycast(viewPoint.position, viewPoint.forward, out RaycastHit hit, attackRange, targetLayerMask))
            {
                // ���̰� IDamagable �������̽��� ������ ������Ʈ�� �浹�ߴٸ�
                IDamagable damageable = hit.collider.gameObject.GetComponent<IDamagable>();
                // TakeDamage �Լ��� ȣ���Ͽ� ���ظ� �����ϴ�.
                Debug.Log(hit.collider.gameObject.name);
                damageable?.TakeDamage(deal, transform.position);
                AudioManager.Instance.PlaySfx(AudioManager.SFX.EnemyShoot);
                attackCost = 0;
            }
            Debug.Log("Attacking");
            //owner.attackCost--;
            //owner.StartCoroutine(AttackCoroutine());
        }
    } // �� ����(�þ� ��)
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
    } // �� �̵�
    public void search()
    {
        if ( Vector3.Distance(transform.position, lostPosition) < 2f)
        {
            animator.SetBool("Walk", false);
            alertArrive = true;
        }
    } // �� ������ �����ġ�� �̵�

    private float curSpeed;
    public void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            patrolTarget = patrolTarget == patrolPosition1 ? patrolPosition2 : patrolPosition1;
            arrive = true;
        }
    } // �� ���� ��ǥ
    public IEnumerator PatrolIdle()
    {
        yield return new WaitForSeconds(idleTime);
        arrive = false;
    } // ���� ��� �ð�
    public void Direction()
    {
        if (firstTarget != null)
        {
            // x�� ���� Ÿ�� ���� ���� ���
            Vector3 targetDirection = new Vector3(firstTarget.position.x - transform.position.x, 0.0f, firstTarget.position.z - transform.position.z);

            // ���ʹϾ� ȸ���� ��� (x�ุ ȸ��)
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // �ε巯�� ȸ�� (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // x�� ���� ����� ��ġ ���� ���� ���
            Vector3 targetDirection = new Vector3(lostPosition.x - transform.position.x, 0.0f, lostPosition.z - transform.position.z);

            // ���ʹϾ� ȸ���� ��� (x�ุ ȸ��)
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // �ε巯�� ȸ�� (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    } // �� �ٶ󺸴� ����
    public void DirectionDrone()
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

    }// ���� ȸ�� ������
    public void Line()
    {
        if (firstTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2; // �� ���� �������� ���� ����ϴ�.
            lineRenderer.SetPosition(0, viewPoint.transform.position);
            lineRenderer.SetPosition(1, firstTarget.transform.position);
            timer += Time.deltaTime;
            if (timer >= (isVisible ? showDuration : hideDuration))
            {
                timer = 0f;
                isVisible = !isVisible;

                //lineRenderer Ȱ��ȭ, ��Ȱ��ȭ ���
                lineRenderer.enabled = isVisible;
            }


        }
        else
        {
            lineRenderer.enabled = false;
        }
    } // ����� ������
    public void LaserOn()
    {
        AudioManager.Instance.PlaySfx(AudioManager.SFX.EnemyLockOn);
        playerDetecter2s = GetComponentsInChildren<PlayerDetecter2>();
        foreach (PlayerDetecter2 playerDetecter2 in playerDetecter2s)
        {
            playerDetecter2.targetting = true;
        }
    } // ����� ������
    public void LaserOff()
    {
        playerDetecter2s = GetComponentsInChildren<PlayerDetecter2>();
        foreach (PlayerDetecter2 playerDetecter2 in playerDetecter2s)
        {
            playerDetecter2.targetting = false;
        }
    } // ����� ������
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
        
    }// �� �ݿ� ������ �������� ����
    public void ColorChanger()
    {
        curColor.color = haveColor.MaterialColor();
        // �θ� ������Ʈ ��������
        GameObject parentObject = this.gameObject; // ���� �θ� ������Ʈ�� ��ü

        // �ڽ� ������Ʈ �ݺ� ó��
        foreach (Transform child in parentObject.transform)
        {
            // �ڽ� ������Ʈ�� Renderer ������Ʈ ����
            Renderer renderer = child.GetComponent<Renderer>();

            // Renderer�� �����ϸ� ���� ����
            if (renderer != null)
            {
                renderer.material.color = curColor.color;
            }
        }
    }// ���׸��� �������� ����
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

        // ȸ�� ���� ����
        bool isForward = true;

        while (true)
        {
            // ���� �ٶ󺸰� �ִ� ���� ���
            Vector3 forward = transform.forward;
            forward.y = 0.0f; // Y�� ���� ����

            // ���� ȸ�� ���� ���
            float targetRotationAngle;
            if (isForward)
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) + rotationAngle;
            }
            else
            {
                targetRotationAngle = Vector3.Angle(forward, transform.forward) - rotationAngle;
            }

            // Lerp �Լ��� ����Ͽ� ȸ�� ����
            while (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.1f)
            {
                currentRotationAngle = Mathf.Lerp(currentRotationAngle, targetRotationAngle, securityRotationSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
                yield return null;
            }

            // ȸ�� ���� ����
            isForward = !isForward;

            // ��� ���
            yield return new WaitForSeconds(rotationWait);
            //isRotating = false;
        }
    }
    public void Dead()
    {
        StartCoroutine (DeadCount());
    }
    private IEnumerator DeadCount()
    {
        deathCollider = GetComponentsInChildren<Collider>();
        AudioManager.Instance.PlaySfx(AudioManager.SFX.EnemyDeath);
        agent.speed = 0;
        foreach (Collider collider in deathCollider)
        {
           
            collider.enabled = false;
        }
        yield return new WaitForSeconds(deadDelay);
        
        Destroy(gameObject);
    }
    //public void readyDead()
    //{
    //    regCols = GetComponentInChildren<Collider>(colchild => colchild != gameObject);
    //    foreach (Collider col in regCols)
    //    {
    //        col.enabled = false;
    //    }
    //    regRigids = GetComponentInChildren<Rigidbody>(rigidchild => rigidchild != gameObject);
    //    foreach (Rigidbody rb in regRigids)
    //    {
    //        rb.useGravity = true;
    //        rb.isKinematic = false;
    //    }
    //}
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
    //} // �� ����Ʈ���� Ž���� ����Ʈ ����

