using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class HumanEnemy : EnemyAI, IStunable
{
    private StateMachine stateMachine;
    private void Awake()
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Patrol, new PatrolState(this));
        stateMachine.AddState(State.PatrolIdle, new PatrolIdleState(this));
        stateMachine.AddState(State.Trace, new TraceState(this));
        stateMachine.AddState(State.Groggy, new GroggyState(this));
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
        Debug.Log("stunstate");
        stateMachine.ChangeState(State.Groggy);
    }
    IEnumerator StunCoroutine()
    {
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
    private class HumanEnemyState : BaseState
    {
        protected HumanEnemy owner;
        protected Transform transform => owner.transform;
        protected float attackRange => owner.attackRange;
        
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
        

        public HumanEnemyState(HumanEnemy owner)
        {
            this.owner = owner;
        }


    }
    private class IdleState : HumanEnemyState
    {

        public IdleState(HumanEnemy owner) : base(owner) { }
        public override void Enter()
        {
            owner.addTargetRange = owner.idleRange;
            owner.agent.speed = owner.patrolSpeed;
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
                owner.patrolTarget = owner.patrolPosition1;
                ChangeState(State.Patrol);

            }
            else if (firstTarget != null)
            {
                
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }
    }
    private class PatrolState : HumanEnemyState
    {

        public PatrolState(HumanEnemy owner) : base(owner) { }
        public override void Enter()
        {

            owner.animator.SetBool("Walk", true);
            owner.addTargetRange = owner.patrolRange;
            owner.agent.speed = owner.patrolSpeed;
        }
        public override void Update()
        {
            Debug.Log("Patrol");
            owner.ColorChanger();
            owner.FindTarget();
            owner.Patrol();
            owner.Move();
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
                
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
            else if (owner.arrive == true)
            {
                owner.animator.SetBool("Walk", false);
                ChangeState(State.PatrolIdle);
            }
        }
    }
    private class PatrolIdleState : HumanEnemyState
    {
        public PatrolIdleState(HumanEnemy owner) : base(owner) { }
        public override void Enter()
        {
            owner.arrive = true;
            owner.alertArrive = false;
            owner.StartCoroutine(owner.PatrolIdle());
            owner.addTargetRange = owner.patrolRange;
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
                ChangeState(State.Patrol);
            }
            else if (owner.firstTarget != null)
            {
                owner.animator.SetBool("Walk", true);
                owner.agent.speed = owner.TraceSpeed;
                owner.returnPoint = owner.transform.position;
                ChangeState(State.Trace);
            }
        }
    }
    private class TraceState : HumanEnemyState
    {
        public TraceState(HumanEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Trace");
            owner.agent.speed = owner.TraceSpeed;
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
        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
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
    private class GroggyState : HumanEnemyState
    {
        public GroggyState(HumanEnemy owner) : base(owner) { }

        public override void Enter()
        {
            owner.StartCoroutine(owner.StunCoroutine());
        }
        public override void Update()
        {
            Debug.Log("stun!!!");
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
    private class AlertState : HumanEnemyState
    {


        public AlertState(HumanEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Alert start");
            owner.agent.destination = owner.lostPosition;
            owner.addTargetRange = owner.alertRange;
        }
        public override void Update()
        {
            Debug.Log("Alert");
            owner.ColorChanger();
            owner.FindTarget();
            owner.search();
            
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
    private class ReturnState : HumanEnemyState
    {
        public ReturnState(HumanEnemy owner) : base(owner) { }

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
                ChangeState(State.Patrol);
            }
        }
    }



    private class BattleState : HumanEnemyState
    {
        public BattleState(HumanEnemy owner) : base(owner) { }



        public override void Enter()
        {
            owner.addTargetRange = owner.battleRange;
            owner.agent.speed = 0f;
            owner.LaserOn();
        }

        public override void Update()
        {
            owner.ColorChanger();
            owner.FindTarget();
            owner.Direction();
            owner.Attack();
        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                owner.LaserOff();
                ChangeState(State.Die);
            }
            else if (firstTarget == null)
            {
                owner.LaserOff();
                ChangeState(State.Alert);
            }
            else if (Vector3.Distance(firstTarget.position, transform.position) >= attackRange)
            {
                owner.LaserOff();
                ChangeState(State.Trace);
            }
            


        }
    }

    private class DieState : HumanEnemyState
    {
        public DieState(HumanEnemy owner) : base(owner) { }

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

    private class GameoverState : HumanEnemyState
    {
        public GameoverState(HumanEnemy owner) : base(owner) { }


    }
}
