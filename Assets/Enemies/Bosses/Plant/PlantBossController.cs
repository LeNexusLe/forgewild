using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlantBossEnemy : MonoBehaviour, IDamageable, IBoss
{
    public event Action OnBossKilled;

    [Header("Stats")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float meleeRange = 1f;
    private Transform player;

    [Header("Phase Thresholds")]
    public float phase2HPThreshold = 50f;
    private int phase = 1;

    [Header("Attacks")]
    public float meleeDamage = 10f;
    public float meleeCooldown = 1f;

    [Header("Special Attack Ranges")]
    public float vineWhipRange = 5f;

    public float vineWhipCooldown = 5f;
    public GameObject vineWhipPrefab;

    public float aoeCooldown = 8f;
    public GameObject aoePrefab;

    public float spawnCooldown = 15f;
    public GameObject minionPrefab;

    private bool isAttacking = false;

    private float lastMeleeTime;
    private float lastVineWhipTime;
    private float lastAOETime;
    private float lastSpawnTime;

    private bool isPerformingSpecialAttack = false;

    private Rigidbody2D rb;
    private Animator animator;

    [Header("UI")]
    public GameObject bossHealthBarPrefab;
    private Slider slider;

    void Awake()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        GameObject uiCanvas = GameObject.Find("UI");
        if (bossHealthBarPrefab != null)
        {
            GameObject go = Instantiate(bossHealthBarPrefab, uiCanvas.transform);
            slider = go.GetComponent<Slider>();
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
        if (phase == 1 && currentHP <= phase2HPThreshold)
        {
            phase = 2;
        }
    }

    void HandleMovement()
    {
        if (!player) return;
        Vector2 dir = (player.position - transform.position);
        float distance = dir.magnitude;

        if (distance > meleeRange && !isPerformingSpecialAttack)
        {
            rb.linearVelocity = dir.normalized * moveSpeed;
            animator.SetBool("isWalking", true);
            animator.SetFloat("InputX", dir.normalized.x);
            animator.SetFloat("InputY", dir.normalized.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", dir.normalized.x);
            animator.SetFloat("LastInputY", dir.normalized.y);
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleAttacks()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= meleeRange && Time.time >= lastMeleeTime + meleeCooldown && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
            lastMeleeTime = Time.time;
        }

        if (Time.time >= lastVineWhipTime + vineWhipCooldown && distance <= vineWhipRange)
        {
            StartCoroutine(PerformVineWhip());
            lastVineWhipTime = Time.time;
        }

        if (phase == 2)
        {
            if (phase == 2 && Time.time >= lastAOETime + aoeCooldown)
            {
                StartCoroutine(PerformAOE());
                lastAOETime = Time.time;
            }

            if (Time.time >= lastSpawnTime + spawnCooldown)
            {
                SpawnMinion();
                lastSpawnTime = Time.time;
            }
        }
    }

    IEnumerator MeleeAttack()
    {
        isAttacking = true;
        isPerformingSpecialAttack = true;
        rb.linearVelocity = Vector2.zero;

        Vector2 direction = (player.position - transform.position).normalized;

        animator.SetBool("isAttacking", true);
        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);

        yield return new WaitForSeconds(0.3f);

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.TakeDamage(meleeDamage);

        yield return new WaitForSeconds(0.4f);

        animator.SetBool("isAttacking", false);

        isAttacking = false;
        isPerformingSpecialAttack = false;
    }

    IEnumerator PerformVineWhip()
    {
        isPerformingSpecialAttack = true;

        rb.linearVelocity = Vector2.zero;

        Vector2 dir = (player.position - transform.position).normalized;

        float spawnDistance = 2f;
        Vector3 spawnPos = transform.position + (Vector3)(dir * spawnDistance);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);

        Instantiate(vineWhipPrefab, spawnPos, rotation);

        yield return new WaitForSeconds(1.2f);

        isPerformingSpecialAttack = false;
    }

    IEnumerator PerformAOE()
    {

        Vector3 spawnPos = player.position;

        Quaternion rotation = Quaternion.identity;

        Instantiate(aoePrefab, spawnPos, rotation);

        yield return new WaitForSeconds(1.5f);

    }

    void SpawnMinion()
    {
        Instantiate(minionPrefab, transform.position + Vector3.right * 2f, Quaternion.identity);
    }

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        slider.value = currentHP / maxHP;
        if (currentHP <= 0)
        {
            slider.gameObject.SetActive(false);
            OnBossKilled?.Invoke();
            Destroy(gameObject);
        }
    }
}
