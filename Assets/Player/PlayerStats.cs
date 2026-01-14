using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base stats")]
    public float maxHP;
    public float moveSpeed;
    public float attackDamage;
    public float attackCooldown;

    [Header("Runtime stats")]
    public float currentHP;

    private void Awake()
    {
        maxHP = 10f;
        moveSpeed = 5f;
        attackDamage = 1f;
        attackCooldown = 1f;

        currentHP = maxHP;
    }

    public void ModifyCurrentHP(float value)
    {
        currentHP += value;
    }

    public void ModifyMaxHP(float value)
    {
        maxHP += value;
        currentHP += value;
    }

    public void ModifyMoveSpeed(float value)
    {
        moveSpeed += value;
    }

    public void ModifyAttackDamage(float value)
    {
        attackDamage += value;
    }

    public void ModifyAttackCooldown(float value)
    {
        attackCooldown -= value;
    }
    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}
