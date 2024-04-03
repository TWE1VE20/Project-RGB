using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Windows;
using static UnityEngine.UI.GridLayoutGroup;

public class HumanMonster : MonsterAI, IStunable
{
    // 시간 사용시 주의 
    // 슬로우모션 영향 받는건 deltaTime
    // 안받는건 Time.unscaleddeltatime
    public enum State { Idle, Trace, Patrol, Groggy, Avoid, Return, Battle, Die, Gameover }

    [Header("Component")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Rigidbody rigid;
    [SerializeField] GameObject self;

    [Header("Attack")]
    [SerializeField] bool debug;
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] float attackRange;
    [SerializeField] int deal;
    [SerializeField] float attackCost;
    [SerializeField] float attackCooltime;
    [SerializeField] Transform firstTarget;
    [SerializeField] Transform secondTarget;
    [SerializeField] float rotationSpeed;
    private float cosRange;
    Collider[] atkColliders = new Collider[20];
    private Vector3 moveDir;
    private Vector3 myPos;

    [Header("SnipeLine")]
    [SerializeField] float showDuration;
    [SerializeField] float hideDuration;
    [SerializeField] float timer;
    [SerializeField] bool isVisible;
    private LineRenderer lineRenderer;


    [Header("Spec")]
    [SerializeField] int MaxHP;
    [SerializeField] int hp;
    [SerializeField] bool isDied;
    [SerializeField] Transform viewPoint;
    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] float addTargetRange;
    [SerializeField] float traceRange;
    [SerializeField] float avoidRange;
    [SerializeField] bool groggyAble;
    

    [Header("gizmo")]
    [SerializeField, Range(0, 360)] float angle;

    [Header("Move")]
    [SerializeField] NavMeshAgent agent;

    [Header("Patrol")]
    [SerializeField] Transform patorolPoint1;
    [SerializeField] Transform patorolPoint2;
    [SerializeField] Vector3 patrolTarget;
    [SerializeField] Vector3 returnPoint;

    [Header("Speed")]
    [SerializeField] float patrolSpeed;
    [SerializeField] float TraceSpeed;
    [SerializeField] float BattleSpeed;
    [SerializeField] float ReturnSpeed;

    private StateMachine stateMachine;
    private Transform enemyUlti;
    private float preAngle;
    private float cosAngle;

    
    private float CosAngle
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

    

    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Patrol, new PatrolState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Avoid, new AvoidState(this));
        stateMachine.AddState(State.Return, new ReturnState(this));
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.AddState(State.Gameover, new GameoverState(this));
        stateMachine.InitState(State.Idle);

        base.Awakefirst();
    }


    private void Start()
    {
        this.hitPoint = hp;
        
        //ListChoice();
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
    //}
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(viewPoint.position, addTargetRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(viewPoint.position, traceRange);

        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * viewPoint.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * viewPoint.forward;
        // 왜 사라짐?
        Debug.DrawRay(viewPoint.position, rightDir * addTargetRange, Color.cyan);
        Debug.DrawRay(viewPoint.position, leftDir * addTargetRange, Color.cyan);
    }

    public void Stun()
    {
        StartCoroutine(StunCoroutine());
    }
    IEnumerator StunCoroutine()
    {
        stateMachine.ChangeState(State.Groggy);
        yield return new WaitForSeconds(3f);
        stateMachine.ChangeState(State.Idle);
    }

    private class HumanMonsterState : BaseState
    {
        protected HumanMonster owner;
        protected Transform transform => owner.transform;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hitPoint;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Transform enemyUlti => owner.enemyUlti;
        
        protected Transform viewPoint => owner.viewPoint;

        protected float addTargetrange => owner.addTargetRange;
        protected Collider[] atkColliders => owner.atkColliders;
        protected LayerMask targetLayerMask => owner.targetLayerMask;
        protected float cosRange => owner.cosRange;
        protected float CosAngle => owner.CosAngle;
        protected LayerMask obstacleLayerMask => owner.obstacleLayerMask;

        protected LineRenderer lineRenderer => owner.gameObject.GetComponent<LineRenderer>();
        public HumanMonsterState(HumanMonster owner)
        {
            this.owner = owner;
        }
        
        public void FindTarget() // 적 탐색 하는 부분
        {
            if (firstTarget == null)
            {
                int size = Physics.OverlapSphereNonAlloc(viewPoint.position, addTargetrange, atkColliders, targetLayerMask);
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
                        owner.firstTarget = owner.atkColliders[i].transform;
                        
                        owner.moveDir = dirToTarget;
                        return;
                    }
                }
            }
            else if (firstTarget != null)
            {
                float distToTarget = Vector3.Distance(firstTarget.transform.position, viewPoint.position);
                if (distToTarget > owner.traceRange)
                {
                    owner.firstTarget = null;
                }
            }
            
        }
        public void Attack()
        {
            owner.attackCost += Time.deltaTime;
            if (owner.attackCost >= 3f)
            {
                //owner.StopCoroutine(AttackCoroutine());
                //RaycastHit hit; 레이 발사
                if (Physics.Raycast(viewPoint.position, viewPoint.forward, out RaycastHit hit, attackRange, targetLayerMask))
                {
                    // 레이가 IDamagable 인터페이스를 구현한 오브젝트에 충돌했다면
                    IDamagable damageable = hit.collider.gameObject.GetComponent<IDamagable>();
                    // TakeDamage 함수를 호출하여 피해를 입힙니다.
                    Debug.Log(hit.collider.gameObject.name);
                    damageable?.TakeDamage(owner.deal, owner.transform.position);
                    owner.attackCost = 0;
                }
                Debug.Log("Attacking");
                //owner.attackCost--;
                //owner.StartCoroutine(AttackCoroutine());
            }
        }
        public void Move()
        {
            if ( firstTarget != null)
            {
                Debug.Log("moving");
                owner.agent.destination = firstTarget.transform.position;
            }
            if (firstTarget == null)
            {
                owner.agent.destination = owner.patrolTarget;
                return;
            }
        }
        public void Patrol()
        {
            if (Vector3.Distance(transform.position, owner.patrolTarget) < 1f)
            {
                owner.patrolTarget = owner.patrolTarget == owner.patorolPoint1.position ? owner.patorolPoint2.position : owner.patorolPoint1.position;
            }
        }
        public void Direction()
        {
            if (firstTarget !=null)
            {
                //transform.LookAt(firstTarget);
                Quaternion targetRotation = Quaternion.LookRotation(firstTarget.position - viewPoint.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, owner.rotationSpeed * Time.deltaTime);
            }
            else
            {
                return;
            }
            
        }
        public void Line()
        {
            if (firstTarget != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2; // 두 개의 정점으로 선을 만듭니다.
                lineRenderer.SetPosition(0, owner.viewPoint.transform.position);
                lineRenderer.SetPosition(1, firstTarget.transform.position);

                owner.timer += Time.deltaTime;

                if (owner.timer >= (owner.isVisible ? owner.showDuration : owner.hideDuration))
                {
                    owner.timer = 0f;
                    owner.isVisible = !owner.isVisible;

                    //lineRenderer 활성화, 비활성화 토글
                    lineRenderer.enabled = owner.isVisible;
                }


            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }
    private class IdleState : HumanMonsterState
    {

        public IdleState(HumanMonster owner) : base(owner) { }
        public override void Enter()
        {
            owner.addTargetRange = 5f;
        }
        public override void Update()
        {
            FindTarget();
        }
        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                owner.patrolTarget = owner.patorolPoint1.transform.position;
                ChangeState(State.Patrol);
                
            }
            else if (firstTarget != null)
            {
                owner.lineRenderer.enabled = true;
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }


    }
    private class PatrolState : HumanMonsterState
    {

        public PatrolState(HumanMonster owner) : base(owner) { }
        public override void Enter()
        {
            
            owner.animator.SetBool("Walk", true);
            owner.agent.speed = owner.patrolSpeed;
        }
        public override void Update()
        {
            FindTarget();
            Patrol();
            Move();
        }
        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Die);
            }
            else if (owner.firstTarget != null)
            {
                lineRenderer.enabled = true;
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }


    }
    private class TraceState : HumanMonsterState
    {
        public TraceState(HumanMonster owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Trace");
            owner.agent.speed = 4f;
            owner.addTargetRange = owner.traceRange;
            owner.animator.SetBool("Walk", true);
            
        }
        public override void Update()
        {
            Debug.Log("Tracing");
            FindTarget();
            Direction();
            Move();
            Line();

        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                owner.addTargetRange = 5;
                owner.firstTarget = null;
                owner.animator.SetBool("Walk", true);
                ChangeState(State.Return);                
            }
            else if (Vector3.Distance(transform.position, owner.firstTarget.transform.position) <= attackRange)
            {
                owner.addTargetRange = 5;
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Battle);
            }

        }
    }
    private class GroggyState : HumanMonsterState
    {
        public GroggyState(HumanMonster owner) : base(owner) { }

        public override void Enter()
        {
            owner.Stun();
        }
        public override void Update()
        {


        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                owner.StopCoroutine(owner.StunCoroutine());
                owner.animator.Play(0);
                ChangeState(State.Die);
            }
        }
    }
    private class AvoidState : HumanMonsterState
    {


        public AvoidState(HumanMonster owner) : base(owner) { }

        public override void Enter()
        {

        }
        public override void Update()
        {
           

        }

        public override void Transition()
        {
            
        }
    }
    private class ReturnState : HumanMonsterState
    {


        public ReturnState(HumanMonster owner) : base(owner) { }

        public override void Enter()
        {
            owner.animator.SetBool("Walk", true);
            Debug.Log("Return");
            owner.firstTarget = null;
            owner.agent.speed = owner.ReturnSpeed;
            owner.agent.destination = owner.returnPoint;
            
            
        }
        public override void Update()
        {

        }

        public override void Transition()
        {
            if (Vector3.Distance(transform.position, owner.returnPoint) < 0.1f)
            {
                owner.haveColor.SetColor(owner.InitColor);
                owner.returnPoint = new Vector3(0, 0, 0);
                owner.agent.speed = 3;
                ChangeState(State.Patrol);
            }
        }
    }



    private class BattleState : HumanMonsterState
    {
        public BattleState(HumanMonster owner) : base(owner) { }



        public override void Enter()
        {
            owner.addTargetRange = owner.ReturnSpeed;
            owner.agent.speed = owner.BattleSpeed;
        }

        public override void Update()
        {
            FindTarget();
            Direction();
            Attack();
            Line();

        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (Vector3.Distance(firstTarget.position, transform.position) >= attackRange)
            {
                ChangeState(State.Trace);
            }
            else if (firstTarget == null)
            {
                owner.lineRenderer.enabled = false;
                ChangeState(State.Return);
            }
            

        }
    }

    private class DieState : HumanMonsterState
    {
        public DieState(HumanMonster owner) : base(owner) { }

        public override void Enter()
        {
            
            owner.animator.Play(0);
        }
        public override void Update()
        {
            
        }
        public override void Transition()
        {
            
        }
    }

    private class GameoverState : HumanMonsterState
    {
        public GameoverState(HumanMonster owner) : base(owner) { }


    }
}
