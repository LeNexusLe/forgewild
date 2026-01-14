using UnityEngine;

public class FireSlimeMinion : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float maxHP = 25f;
    float currentHP;

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    Transform player;
    Rigidbody2D rb;
    Animator animator;

    [Header("Fireball Attack")]
    public GameObject fireballPrefab;
    public float fireballCooldown = 2.5f;
    public float projectileSpeed = 6f;
    float lastFireball;

    [Header("Death Explosion")]
    public GameObject explosionPrefab;

    FireSlimeBoss boss;

    void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void Init(FireSlimeBoss owner)
    {
        boss = owner;
    }

    void Update()
    {
        if (!player) return;

        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        if (animator)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
        }
    }

    void HandleAttack()
    {
        if (Time.time < lastFireball + fireballCooldown) return;

        Vector2 dir = (player.position - transform.position).normalized;

        GameObject fb = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fb.GetComponent<Rigidbody2D>().linearVelocity = dir * projectileSpeed;

        lastFireball = Time.time;
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        boss?.OnMinionKilled();

        Destroy(gameObject);
    }
}
