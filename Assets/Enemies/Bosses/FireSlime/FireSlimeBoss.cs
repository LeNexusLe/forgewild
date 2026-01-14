using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FireSlimeBoss : MonoBehaviour, IDamageable, IBoss
{
    public event Action OnBossKilled;

    [Header("Stats")]
    public float maxHP = 150f;
    float currentHP;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float meleeRange = 1.2f;
    Transform player;
    Rigidbody2D rb;
    Animator animator;

    [Header("Phases")]
    public float phase2HP = 90f;
    public float phase3HP = 40f;
    int phase = 1;

    [Header("AOE Attack")]
    public float aoeCooldown = 4f;
    public GameObject aoePrefab;
    float lastAOE;

    [Header("Fireball Shot")]
    public float fireballCooldown = 3f;
    public float projectileSpeed = 6f;
    public GameObject fireballPrefab;
    float lastFireball;

    [Header("Dash Attack (Phase 2+)")]
    public float dashCooldown = 6f;
    public float dashDuration = 1.5f;
    public float dashSpeed = 5f;
    public float dashDistance = 4f;
    float lastDash;

    [Header("Phase 3 Split")]
    public GameObject smallSlimePrefab;
    bool hasSplit = false;
    int aliveMinions = 0;

    [Header("UI")]
    public GameObject bossHealthBarPrefab;
    Slider slider;

    bool isAttacking = false;

    void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        GameObject ui = GameObject.Find("UI");
        if (bossHealthBarPrefab)
        {
            GameObject bar = Instantiate(bossHealthBarPrefab, ui.transform);
            slider = bar.GetComponent<Slider>();
            slider.value = 1f;
            slider.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!player) return;

        HandlePhase();
        HandleMovement();
        HandleAttacks();
    }

    void HandlePhase()
    {
        if (phase == 1 && currentHP <= phase2HP)
            phase = 2;

        if (phase == 2 && currentHP <= phase3HP && !hasSplit)
            SplitIntoSlimes();
    }

    void HandleMovement()
    {
        if (isAttacking) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        animator.SetBool("isWalking", true);
        animator.SetFloat("InputX", dir.x);
        animator.SetFloat("InputY", dir.y);
    }

    void HandleAttacks()
    {
        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (Time.time >= lastAOE + aoeCooldown && distance <= 3f)
        {
            StartCoroutine(AOE());
            lastAOE = Time.time;
        }

        if (Time.time >= lastFireball + fireballCooldown)
        {
            FireballShot();
            lastFireball = Time.time;
        }

        if (phase >= 2 && distance >= 4f && Time.time >= lastDash + dashCooldown)
        {
            StartCoroutine(DashAttack());
            lastDash = Time.time;
        }
    }

    IEnumerator AOE()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        Instantiate(aoePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    void FireballShot()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        float[] angles = { -20f, 0f, 20f };

        foreach (float angle in angles)
        {
            Vector2 shootDir = Quaternion.Euler(0, 0, angle) * dir;
            GameObject fb = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fb.GetComponent<Rigidbody2D>().linearVelocity = shootDir * projectileSpeed;
        }
    }

    IEnumerator DashAttack()
    {
        isAttacking = true;
        lastDash = Time.time;

        Vector2 direction = (player.position - transform.position).normalized;

        float time = 0f;

        while (time < dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        isAttacking = false;
    }

    void SplitIntoSlimes()
    {
        hasSplit = true;

        GameObject s1 = Instantiate(smallSlimePrefab, transform.position + Vector3.left, Quaternion.identity);
        GameObject s2 = Instantiate(smallSlimePrefab, transform.position + Vector3.right, Quaternion.identity);

        aliveMinions = 2;

        s1.GetComponent<FireSlimeMinion>().Init(this);
        s2.GetComponent<FireSlimeMinion>().Init(this);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
        isAttacking = true;
        slider.gameObject.SetActive(false);
    }

    public void OnMinionKilled()
    {
        aliveMinions--;

        if (aliveMinions <= 0)
        {
            OnBossKilled?.Invoke();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        slider.value = currentHP / maxHP;
    }
}
