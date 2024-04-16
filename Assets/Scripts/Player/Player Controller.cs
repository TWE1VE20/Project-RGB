using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static AudioManager;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] PlayerPauseController pauseController;

    [Header("Spec")]
    [SerializeField] float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float MaxFallSpeed;
    [SerializeField] float gravity;

    [Header("Sound")]
    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] AudioClip[] playerAudioClips;
    [SerializeField] AudioManager audioManager;
    private IEnumerator soundCorutine;
    private bool isRunSound;


    private Vector3 moveDir;
    public float ySpeed { get; private set; }
    private bool isRun;

    public bool IsAlive;

    void Start()
    {
        IsAlive = true;
        isRunSound = false;
    }

    void Update()
    {
        if (IsAlive)
        {
            Move();
            JumpMove();
        }
    }

    private void Move()
    {
        if (isRun && groundChecker.isGround)
        {
            if (playerAudioSource.clip != playerAudioClips[1])
                playerAudioSource.clip = playerAudioClips[1];
            controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
            controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
            // animator.SetFloat("MoveSpeed", moveDir.magnitude * moveSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("XSpeed", moveDir.x * moveSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("YSpeed", moveDir.z * moveSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            if (playerAudioSource.clip != playerAudioClips[0])
                playerAudioSource.clip = playerAudioClips[0];
            controller.Move(transform.right * moveDir.x * walkSpeed * Time.deltaTime);
            controller.Move(transform.forward * moveDir.z * walkSpeed * Time.deltaTime);
            // animator.SetFloat("MoveSpeed", moveDir.magnitude * walkSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("XSpeed", moveDir.x * walkSpeed, 0.1f, Time.deltaTime);
            // animator.SetFloat("YSpeed", moveDir.z * walkSpeed, 0.1f, Time.deltaTime);
        }


        Debug.Log(moveDir.y);
        Debug.Log(moveDir.z);

        if (groundChecker.isGround)
        {
            if (playerAudioSource.isPlaying == false && (moveDir.x > 0.01 || moveDir.x < -0.01 || moveDir.z > 0.01 || moveDir.z < -0.01))
            {
                if (soundCorutine == null)
                {
                    soundCorutine = MoveSound(0.4f);
                    StartCoroutine(soundCorutine);
                }
                Debug.Log("Play");
            }
            else if (playerAudioSource.isPlaying == true && moveDir.x < 0.01 && moveDir.x > -0.01 && moveDir.z < 0.01 && moveDir.z > -0.01)
            {
                if (soundCorutine != null)
                {
                    StopCoroutine(soundCorutine);
                    soundCorutine = null;
                }
                playerAudioSource.Stop();
                Debug.Log("Stop");
            }
        }
        else
        {
            if (soundCorutine != null)
            {
                StopCoroutine(soundCorutine);
                soundCorutine = null;
            }
            playerAudioSource.Stop();
            Debug.Log("Stop");
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

    private void OnPause(InputValue value)
    {
        if (IsAlive)
        {
            if (!Manager.timeflow.Paused)
                pauseController.Paused();
            else
                pauseController.PauseEnd();
            Manager.timeflow.Paused = !Manager.timeflow.Paused;
        }
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    public void DeathSound()
    {
        Debug.Log("Dead Sound");
        audioManager.StopBgm(BGM.InGame);
        if(soundCorutine != null)
            StopCoroutine(soundCorutine);
        playerAudioSource.clip = playerAudioClips[2];
        playerAudioSource.loop = false;
        playerAudioSource.Play();
    }

    IEnumerator MoveSound(float time)
    {
        while (true)
        {
            playerAudioSource.Play();
            yield return new WaitForSeconds(time);
        }
    }
}
