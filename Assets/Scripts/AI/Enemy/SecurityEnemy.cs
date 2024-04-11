using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityEnemy : EnemyAI, IStunable
{
    private StateMachine stateMachine;
    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Patrol, new PatrolState(this));
        stateMachine.AddState(State.PatrolIdle, new PatrolIdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Alert, new AlertState(this));
        stateMachine.AddState(State.Return, new ReturnState(this));
        stateMachine.AddState(State.Battle, new BattleState(this));
        stateMachine.AddState(State.Die, new DieState(this));
        stateMachine.AddState(State.Gameover, new GameoverState(this));
        stateMachine.InitState(State.Idle);

        base.Awakefirst();
    }


    private void Start()
    {

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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(viewPoint.position, addTargetRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(viewPoint.position, traceRange);

        Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * viewPoint.forward;
        Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * viewPoint.forward;
        // �� �����?
        Debug.DrawRay(viewPoint.position, rightDir * addTargetRange, Color.cyan);
        Debug.DrawRay(viewPoint.position, leftDir * addTargetRange, Color.cyan);
    }
    private class SecurityEnemyState : BaseState
    {
        protected SecurityEnemy owner;
        protected Transform transform => owner.transform;
        protected float attackRange => owner.attackRange;
        protected float avoidRange => owner.avoidRange;
        protected float hp => owner.hp;

        protected Animator animator => owner.animator;
        protected Transform firstTarget => owner.firstTarget;
        protected Transform viewPoint => owner.viewPoint;

        protected float addTargetrange => owner.addTargetRange;
        protected Collider[] atkColliders => owner.atkColliders;
        protected LayerMask targetLayerMask => owner.targetLayerMask;
        protected float cosRange => owner.cosRange;
        protected float CosAngle => owner.CosAngle;
        protected LayerMask obstacleLayerMask => owner.obstacleLayerMask;
        protected LineRenderer lineRenderer => owner.gameObject.GetComponent<LineRenderer>();

        public SecurityEnemyState(SecurityEnemy owner)
        {
            this.owner = owner;
        }


    }
    private class IdleState : SecurityEnemyState
    {

        public IdleState(SecurityEnemy owner) : base(owner) { }
        public override void Enter()
        {
            owner.addTargetRange = 5f;
        }
        public override void Update()
        {
            owner.ColorChanger();
            owner.FindTarget();
        }
        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                ChangeState(State.Idle);
            }
            else if (firstTarget != null)
            {
                owner.lineRenderer.enabled = true;
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }
    }
    private class PatrolState : SecurityEnemyState
    {

        public PatrolState(SecurityEnemy owner) : base(owner) { }
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
    private class PatrolIdleState : SecurityEnemyState
    {
        public PatrolIdleState(SecurityEnemy owner) : base(owner) { }
        public override void Enter()
        {
            owner.arrive = true;
            owner.alertArrive = false;
            owner.StartCoroutine(owner.PatrolIdle());
            owner.agent.speed = 0;
        }
        public override void Update()
        {
            Debug.Log("PatrolIdle");
            owner.ColorChanger();
            owner.FindTarget();
        }
        public override void Transition()
        {
            if (owner.arrive == false)
            {
                owner.animator.SetBool("Walk", true);
                owner.agent.speed = owner.patrolSpeed;
                ChangeState(State.Idle);
            }
            else if (owner.firstTarget != null)
            {
                owner.animator.SetBool("Walk", true);
                lineRenderer.enabled = true;
                owner.agent.speed = owner.TraceSpeed;
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }
    }
    private class TraceState : SecurityEnemyState
    {
        public TraceState(SecurityEnemy owner) : base(owner) { }

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
            owner.ColorChanger();
            owner.FindTarget();
            owner.Direction();
            owner.Move();
            owner.Line();
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
                ChangeState(State.Alert);
            }
            else if (Vector3.Distance(transform.position, owner.firstTarget.transform.position) <= attackRange)
            {
                owner.addTargetRange = 5;
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Battle);
            }

        }
    }
    private class GroggyState : SecurityEnemyState
    {
        public GroggyState(SecurityEnemy owner) : base(owner) { }

        public override void Enter()
        {
            owner.Stun();
        }
        public override void Update()
        {
            Debug.Log("stun!!");
            owner.ColorChanger();
        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                owner.StopCoroutine(owner.StunCoroutine());
                //owner.animator.Play(0);
                ChangeState(State.Die);
            }
        }
    }
    private class AlertState : SecurityEnemyState
    {


        public AlertState(SecurityEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Alert start");
            owner.agent.destination = owner.lostPosition;
        }
        public override void Update()
        {
            Debug.Log("Alert");
            owner.ColorChanger();
            owner.FindTarget();
            owner.search();
            owner.Line();
        }
        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget != null)
            {
                ChangeState(State.Battle);
            }
            else if (owner.alertArrive == true && owner.firstTarget == null)
            {
                owner.lostPosition = new Vector3(0, 0, 0);
                Debug.Log("Alert to patrolIdle");
                ChangeState(State.PatrolIdle);
            }
        }
    }
    private class ReturnState : SecurityEnemyState
    {


        public ReturnState(SecurityEnemy owner) : base(owner) { }

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
            owner.ColorChange();
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



    private class BattleState : SecurityEnemyState
    {
        public BattleState(SecurityEnemy owner) : base(owner) { }



        public override void Enter()
        {
            owner.addTargetRange = owner.ReturnSpeed;
            owner.agent.speed = 0f;
        }

        public override void Update()
        {
            owner.ColorChanger();
            owner.FindTarget();
            owner.Direction();
            owner.Attack();
            owner.Line();

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
                Debug.Log("why");
                owner.lineRenderer.enabled = false;
                ChangeState(State.Alert);
            }


        }
    }

    private class DieState : SecurityEnemyState
    {
        public DieState(SecurityEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Dead");
            Destroy(owner.gameObject);
        }
        public override void Update()
        {
            owner.ColorChanger();
        }
        public override void Transition()
        {

        }
    }

    private class GameoverState : SecurityEnemyState
    {
        public GameoverState(SecurityEnemy owner) : base(owner) { }


    }
}
