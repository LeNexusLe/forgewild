using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats stats;
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 lastMove;
    public Transform Aim;
    bool isWalking = false;
    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * stats.moveSpeed;

        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * moveInput.x + Vector3.down * moveInput.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);

        }

        if(isWalking && !playingFootsteps)
        {
            StartFootSteps();
        }
        else if(!isWalking)
        {
            StopFootSteps();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.normalized.x);
            animator.SetFloat("LastInputY", moveInput.normalized.y);
            isWalking = false;
            Vector3 vector3 = Vector3.left * lastMove.x + Vector3.down * lastMove.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
            isWalking = true;
            lastMove = moveInput;
        }
    }

    void StartFootSteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0, footstepSpeed);
    }

    void StopFootSteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }

    void PlayFootstep()
    {
        SoundEffectManager.Play("PlayerSteps");
    }
}
