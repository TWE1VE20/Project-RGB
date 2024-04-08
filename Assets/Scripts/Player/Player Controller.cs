using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] GroundChecker groundChecker;

    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float MaxFallSpeed;
    [SerializeField] float gravity;


    private Vector3 moveDir;
    public float ySpeed { get; private set; }
    private bool isRun;

    public bool IsAlive;

    void Start()
    {
        IsAlive = true;
    }

    void Update()
    {
        Move();
        JumpMove();
    }

    private void Move()
    {
        if (isRun && controller.isGrounded)
        {
            controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
            controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
            // animator.SetFloat("MoveSpeed", moveDir.magnitude * moveSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("XSpeed", moveDir.x * moveSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("YSpeed", moveDir.z * moveSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            controller.Move(transform.right * moveDir.x * walkSpeed * Time.deltaTime);
            controller.Move(transform.forward * moveDir.z * walkSpeed * Time.deltaTime);
            // animator.SetFloat("MoveSpeed", moveDir.magnitude * walkSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("XSpeed", moveDir.x * walkSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("YSpeed", moveDir.z * walkSpeed, 0.1f, Time.deltaTime);
        }
    }

    private void JumpMove()
    {
        if (ySpeed > -MaxFallSpeed)
            ySpeed += -gravity * Time.deltaTime;
        //ySpeed += Physics.gravity.y * Time.deltaTime;
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void OnJump(InputValue value)
    {
        if (groundChecker.isGround)
            ySpeed = jumpSpeed;
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
            isRun = true;
        else
            isRun = false;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }
}
