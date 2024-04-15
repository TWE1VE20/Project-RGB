using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class SecurityDroneEnemy : EnemyAI, IStunable
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
        initialLocalRotation = transform.localRotation;
        returnPoint = transform.position;
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
        // ¿Ö »ç¶óÁü?
        Debug.DrawRay(viewPoint.position, rightDir * addTargetRange, Color.cyan);
        Debug.DrawRay(viewPoint.position, leftDir * addTargetRange, Color.cyan);
    }
    private class SecurityDroneEnemyState : BaseState
    {
        protected SecurityDroneEnemy owner;
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

        public SecurityDroneEnemyState(SecurityDroneEnemy owner)
        {
            this.owner = owner;
        }


    }
    private class IdleState : SecurityDroneEnemyState
    {

        public IdleState(SecurityDroneEnemy owner) : base(owner) { }
        public override void Enter()
        {
            transform.localRotation = owner.initialLocalRotation;
            owner.addTargetRange = owner.idleRange;
            owner.agent.speed = owner.patrolSpeed;
            AudioManager.Instance.PlaySfx(AudioManager.SFX.EnemyIdle);
        }
        public override void Update()
        {

            Debug.Log("Idle");
            owner.ColorChange();
            owner.FindTarget();
            owner.Security();// ¹¹ ¿Ö
        }
        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                owner.StopAllCoroutines();
                ChangeState(State.Die);
            }
            else if (firstTarget != null)
            {
                owner.StopAllCoroutines();
                ChangeState(State.Trace);
            }
        }
    }
    private class PatrolState : SecurityDroneEnemyState
    {

        public PatrolState(SecurityDroneEnemy owner) : base(owner) { }
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
    private class PatrolIdleState : SecurityDroneEnemyState
    {
        public PatrolIdleState(SecurityDroneEnemy owner) : base(owner) { }
        public override void Enter()
        {
            owner.animator.SetBool("Walk", false);
            owner.arrive = true;
            owner.addTargetRange = owner.patrolRange;
            owner.agent.speed = 0;
            owner.StartCoroutine(owner.PatrolIdle());

        }
        public override void Update()
        {
            Debug.Log("PatrolIdle");
            owner.ColorChange();
            owner.FindTarget();
        }
        public override void Transition()
        {
            if (owner.arrive == false)
            {
                owner.animator.SetBool("Walk", true);
                owner.agent.speed = owner.TraceSpeed;
                owner.agent.destination = owner.returnPoint;
                ChangeState(State.Return);
            }
            else if (owner.firstTarget != null)
            {
                owner.animator.SetBool("Walk", true);
                owner.agent.speed = owner.TraceSpeed;
                ChangeState(State.Trace);
            }
        }
    }
    private class TraceState : SecurityDroneEnemyState
    {
        public TraceState(SecurityDroneEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Trace");
            owner.addTargetRange = owner.traceRange;
            owner.agent.speed = owner.TraceSpeed;
            owner.animator.SetBool("Walk", true);
        }
        public override void Update()
        {
            Debug.Log("Tracing");
            owner.ColorChange();
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
                owner.animator.SetBool("Walk", true);
                ChangeState(State.Alert);
            }
            else if (Vector3.Distance(transform.position, owner.firstTarget.transform.position) < attackRange)
            {
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Battle);
            }

        }
    }
    private class GroggyState : SecurityDroneEnemyState
    {
        public GroggyState(SecurityDroneEnemy owner) : base(owner) { }

        public override void Enter()
        {
            owner.Stun();
        }
        public override void Update()
        {
            Debug.Log("stun!!");
            owner.ColorChange();
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
    private class AlertState : SecurityDroneEnemyState
    {
        public AlertState(SecurityDroneEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Alert start");
            
            owner.agent.destination = owner.lostPosition;

            owner.addTargetRange = owner.alertRange;
            owner.agent.speed = owner.TraceSpeed;

            owner.alertArrive = false;
        }
        public override void Update()
        {
            Debug.Log("Alert");
            owner.ColorChange();
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
            else if (owner.alertArrive == true)
            {
                Debug.Log("Alert to patrolIdle");
                ChangeState(State.PatrolIdle);
            }
        }
    }
    private class ReturnState : SecurityDroneEnemyState
    {


        public ReturnState(SecurityDroneEnemy owner) : base(owner) { }

        public override void Enter()
        {
            owner.animator.SetBool("Walk", true);
            Debug.Log("Return");
            owner.firstTarget = null;
            owner.agent.speed = owner.ReturnSpeed;
            owner.addTargetRange = owner.alertRange;
            owner.agent.destination = owner.returnPoint;
        }
        public override void Update()
        {
            owner.ColorChange();
        }

        public override void Transition()
        {
            if (Vector3.Distance(transform.position, owner.returnPoint) < 0.5f)
            {
                owner.haveColor.SetColor(owner.InitColor);
                owner.agent.speed = 0f;
                owner.animator.SetBool("Walk", false);
                ChangeState(State.Idle);
            }
        }
    }



    private class BattleState : SecurityDroneEnemyState
    {
        public BattleState(SecurityDroneEnemy owner) : base(owner) { }



        public override void Enter()
        {
            owner.addTargetRange = owner.battleRange;
            owner.agent.speed = 0f;
            owner.LaserOn();
        }

        public override void Update()
        {
            Debug.Log("Battle");
            owner.ColorChange();
            owner.FindTarget();
            owner.Direction();
            owner.Attack();
        }

        public override void Transition()
        {
            if (owner.haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
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

    private class DieState : SecurityDroneEnemyState
    {
        public DieState(SecurityDroneEnemy owner) : base(owner) { }

        public override void Enter()
        {
            Debug.Log("Dead");
            owner.Dead();
            Destroy(owner.gameObject);
        }
        public override void Update()
        {
            owner.ColorChange();
        }
        public override void Transition()
        {

        }
    }

    private class GameoverState : SecurityDroneEnemyState
    {
        public GameoverState(SecurityDroneEnemy owner) : base(owner) { }
    }
}
