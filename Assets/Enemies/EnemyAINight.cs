using UnityEngine;

public class EnemyAINight : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float minRangeDistance = 2f;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Animator animator;

    [HideInInspector] public bool isDashing;

    private EnemyAttack_Melee meleeAttack;
    private EnemyAttack_Range rangeAttack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        meleeAttack = GetComponent<EnemyAttack_Melee>();
        rangeAttack = GetComponent<EnemyAttack_Range>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (!target) return;

        if (isDashing)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        Vector2 dir = target.position - transform.position;
        float distance = dir.magnitude;

        if (rangeAttack != null && distance < minRangeDistance)
        {
            moveDirection = -dir.normalized;
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveDirection.x);
            animator.SetFloat("InputY", moveDirection.y);
        }
        else if ((meleeAttack != null && distance <= 0.1f) || (rangeAttack != null && distance <= rangeAttack.attackRange))
        {
            moveDirection = Vector2.zero;
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", dir.normalized.x);
            animator.SetFloat("LastInputY", dir.normalized.y);
        }
        else
        {
            moveDirection = dir.normalized;
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", moveDirection.x);
            animator.SetFloat("InputY", moveDirection.y);
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}
