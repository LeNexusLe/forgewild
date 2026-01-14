using UnityEngine;
using System.Collections;

public class EnemyAttack_Range : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float attackRange = 5f;
    public float cooldown = 2f;
    public float projectileSpeed = 5f;
    public int damage = 1;

    private float lastAttackTime;
    private Transform target;
    private Animator animator;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!target) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + cooldown)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        lastAttackTime = Time.time;

        Vector2 direction = (target.position - transform.position).normalized;

        animator.SetBool("isAttacking", true);
        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);

        yield return new WaitForSeconds(0.1f);

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * projectileSpeed;

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
            projScript.damage = damage;

        yield return new WaitForSeconds(0.2f);

        animator.SetBool("isAttacking", false);
    }
}
