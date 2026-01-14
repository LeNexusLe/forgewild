using UnityEngine;

public enum StatType
{
    CurrentHP,
    MaxHP,
    MoveSpeed,
    AttackDamage,
    AttackCooldown
}

[System.Serializable]
public class StatModifier
{
    public StatType stat;
    public float value;

    public void Apply(PlayerStats stats)
    {
        switch (stat)
        {
            case StatType.CurrentHP:
                stats.ModifyCurrentHP(value);
                break;
            case StatType.MaxHP:
                stats.ModifyMaxHP(value);
                break;
            case StatType.MoveSpeed:
                stats.ModifyMoveSpeed(value);
                break;
            case StatType.AttackDamage:
                stats.ModifyAttackDamage(value);
                break;
            case StatType.AttackCooldown:
                stats.ModifyAttackCooldown(value);
                break;
        }
    }

}
