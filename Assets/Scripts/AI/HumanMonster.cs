using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;
using static UnityEngine.UI.GridLayoutGroup;

public class HumanMonster : MonsterAI
{
    // 시간 사용시 주의 
    // 슬로우모션 영향 받는건 deltaTime
    // 안받는건 Time.unscaleddeltatime
    public enum State { Idle, Trace, Patrol, Avoid, Return, Battle, Die, Gameover }

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
    [SerializeField] int attackCost;
    [SerializeField] float attackCooltime;
    [SerializeField] Transform firstTarget;
    [SerializeField] Transform secondTarget;
    private Vector3 moveDir;


    private float cosRange;

    Collider[] atkColliders = new Collider[20];


    [Header("Spec")]
    [SerializeField] float avoidRange;
    [SerializeField] int hp;
    [SerializeField] bool isDied;
    [SerializeField] Transform viewPoint;
    [SerializeField] LayerMask obstacleLayerMask;
    [SerializeField] float addTargetRange;
    [SerializeField] int MaxHP;
    [SerializeField] float traceRange;

    [Header("gizmo")]
    [SerializeField, Range(0, 360)] float angle;

    [Header("Move")]
    [SerializeField] NavMeshAgent agent;
    private float ySpeed;

    [Header("Patrol")]
    [SerializeField] Transform patorolPoint1;
    [SerializeField] Transform patorolPoint2;
    [SerializeField] Vector3 patrolTarget;
    [SerializeField] Vector3 returnPoint;

    //private Vector2 moveDir;
    //private float xSpeed;



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

    //private List<BattleAI> enemyList;
    //private float preAngle;


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
        
        public HumanMonsterState(HumanMonster owner)
        {
            this.owner = owner;
        }
        
        public void FindTarget() // 적 탐색 하는 부분
        {
            int size = Physics.OverlapSphereNonAlloc(viewPoint.position, addTargetrange, atkColliders, targetLayerMask);
            
            if (firstTarget == null)
            {
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
        IEnumerator AttackCoroutine()
        {
            while (owner.attackCost == 0)
            {
                yield return new WaitForSeconds(owner.attackCooltime);
                owner.attackCost = 1;
            }
        }
        public void Attack()
        {
            if (owner.attackCost == 1)
            {
                owner.StopCoroutine(AttackCoroutine());
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, targetLayerMask))
                {
                    // 레이가 IDamagable 인터페이스를 구현한 오브젝트에 충돌했다면
                    IDamagable damageable = hit.collider.gameObject.GetComponent<IDamagable>();
                    if (damageable != null)
                    {
                        // TakeDamage 함수를 호출하여 피해를 입힙니다.
                        damageable.TakeDamage(owner.deal);
                    }
                }
                owner.attackCost--;
                owner.StartCoroutine(AttackCoroutine());
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
    }
    private class IdleState : HumanMonsterState
    {

        public IdleState(HumanMonster owner) : base(owner) { }
        public override void Enter()
        {
            
        }
        public override void Update()
        {
            FindTarget();
        }
        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                owner.patrolTarget = owner.patorolPoint1.transform.position;
                ChangeState(State.Patrol);
                
            }
            else if (Vector3.Distance(firstTarget.position, transform.position) < owner.addTargetRange)
            {
                owner.patrolTarget = owner.patorolPoint1.transform.position;
                Debug.Log("returnPoint select");
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }

            else if (Vector3.Distance(firstTarget.position, transform.position) <= attackRange && owner.attackCost == 1)
            {
                Debug.Log("returnPoint select");
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Battle);
            }
        }


    }
    private class PatrolState : HumanMonsterState
    {

        public PatrolState(HumanMonster owner) : base(owner) { }
        public override void Enter()
        {
            
            owner.animator.SetBool("Walk", true);
            owner.agent.speed = 3f;
        }
        public override void Update()
        {
            FindTarget();
            Patrol();
            Move();
        }
        public override void Transition()
        {
            if (hp <= 0)
            {
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Die);
            }
            else if (owner.firstTarget != null)
            {
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
            
        }
        public override void Update()
        {
            FindTarget();
            Move();
        }

        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                ChangeState(State.Return);                
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) <= attackRange)
            {
                ChangeState(State.Battle);
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
            Debug.Log("Return");
            owner.firstTarget = null;
            owner.agent.speed = 5;
            owner.agent.destination = owner.returnPoint;
        }
        public override void Update()
        {

        }

        public override void Transition()
        {
            if (Vector3.Distance(transform.position, owner.returnPoint) < 0.1f)
            {
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
            owner.returnPoint = owner.transform.position;
        }

        public override void Update()
        {
            FindTarget();
            Attack();
        }

        public override void Transition()
        {
            if (hp <= 0)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null || (owner.attackCost == 0))
            {
                ChangeState(State.Idle);
            }
            else if (Vector2.Distance(firstTarget.position, transform.position) > attackRange)
            {
                ChangeState(State.Trace);
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
