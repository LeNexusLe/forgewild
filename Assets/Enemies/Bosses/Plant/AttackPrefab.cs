using UnityEngine;

public class AttackPrefab : MonoBehaviour
{
    [Header("Settings")]
    public float warningTime = 1f;
    public float attackDuration = 0.5f;
    public int damage = 10;

    [Header("Animation")]
    public string attackAnimationName;

    public Color warningColor = Color.red;

    private SpriteRenderer sr;
    private Collider2D col;
    private Animator animator;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        col.enabled = false;
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        sr.color = warningColor;
        yield return new WaitForSeconds(warningTime);

        sr.color = Color.white;
        col.enabled = true;
        animator.enabled = true;
        animator.Play(attackAnimationName, 0, 0f);

        yield return new WaitForSeconds(attackDuration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(damage);
    }
}
