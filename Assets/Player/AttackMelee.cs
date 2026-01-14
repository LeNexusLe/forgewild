using UnityEngine;
using UnityEngine.InputSystem;

public class AttackMelee : MonoBehaviour
{
    [Header("Attack")]
    public GameObject Melee;
    public float atkDuration = 0.3f;
    public Transform Aim;


    private PlayerStats stats;
    private Animator animator;

    private bool isHoldingAttack = false;
    private bool isAttacking = false;
    private bool canAttack = true;

    private float atkTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canAttack)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= stats.attackCooldown)
            {
                cooldownTimer = 0f;
                canAttack = true;
            }
        }

        if (isHoldingAttack && !isAttacking && canAttack)
            StartAttack();

        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
                EndAttack();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started) isHoldingAttack = true;

        if (context.canceled) isHoldingAttack = false;
    }

    void StartAttack()
    {
        isAttacking = true;
        canAttack = false;
        atkTimer = 0f;

        Vector2 direction = -Aim.up.normalized;

        animator.SetBool("isAttacking", true);
        animator.SetFloat("AttackX", direction.x);
        animator.SetFloat("AttackY", direction.y);

        Melee.SetActive(true);
    }

    void EndAttack()
    {
        isAttacking = false;
        atkTimer = 0f;

        animator.SetBool("isAttacking", false);
        Melee.SetActive(false);
    }
}
