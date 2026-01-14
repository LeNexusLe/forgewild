using UnityEngine;
using System.Collections;

public class EnemyDashAttack : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.25f;
    public float attackRange = 3f;
    public float cooldown = 1.5f;

    private float lastAttackTime;
    private bool isDashing;

    private Rigidbody2D rb;
    private Transform player;

    private EnemyAIDay dayAI;
    private EnemyAINight nightAI;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        dayAI = GetComponent<EnemyAIDay>();
        nightAI = GetComponent<EnemyAINight>();
    }

    void Update()
    {
        if (isDashing) return;
        if (Time.time < lastAttackTime + cooldown) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastAttackTime = Time.time;

        if (dayAI) dayAI.isDashing = true;
        if (nightAI) nightAI.isDashing = true;

        animator.SetBool("isAttacking", true);

        Vector2 direction = (player.position - transform.position).normalized;

        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);

        float time = 0f;

        while (time < dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isAttacking", false);

        isDashing = false;
        if (dayAI) dayAI.isDashing = false;
        if (nightAI) nightAI.isDashing = false;
    }
}
