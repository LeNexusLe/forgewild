using UnityEngine;

public class EnemyAIDay : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float chaseRange = 20f;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Vector2 spawnPoint;

    public float cooldown = 5f;
    private float timer;

    private Animator animator;

    [HideInInspector] public bool isDashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        spawnPoint = transform.position;
    }

    enum AIState
    {
        Patrol,
        Chase,
        Return
    }

    private AIState currentState;
    private Vector2 patrolPoint;

    void Update()
    {
        if (!target) return;

        if (isDashing)
        {
            animator.SetBool("isWalking", false);
            return;
        }


            float distanceToPlayer = Vector2.Distance(target.position, spawnPoint);
        float distanceToSpawn = Vector2.Distance(transform.position, spawnPoint);

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chase;
        }

        timer -= Time.deltaTime;

        switch (currentState)
        {
            case AIState.Patrol:
                if (timer <= 0)
                {
                    patrolPoint = spawnPoint + Random.insideUnitCircle * chaseRange;
                    timer = cooldown;
                }
                MoveTo(patrolPoint);
                break;

            case AIState.Chase:
                if (distanceToPlayer > chaseRange)
                {
                    currentState = AIState.Return;
                }
                else
                {
                    MoveTo(target.position);
                }
                break;

            case AIState.Return:
                if (distanceToSpawn <= 0.2f)
                {
                    currentState = AIState.Patrol;
                }
                else
                {
                    MoveTo(spawnPoint);
                }
                break;
        }
    }
    void MoveTo(Vector2 destination)
    {
        Vector2 dir = destination - (Vector2)transform.position;

        if (dir.magnitude < 0.2f)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", dir.normalized.x);
            animator.SetFloat("LastInputY", dir.normalized.y);
            moveDirection = Vector2.zero;
        }    
        else
        {
            moveDirection = dir.normalized;
            animator.SetBool("isWalking",true);
            animator.SetFloat("InputX", moveDirection.x);
            animator.SetFloat("InputY", moveDirection.y);
        }
    }

    private void FixedUpdate()
    {
        if (target && !isDashing)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}
