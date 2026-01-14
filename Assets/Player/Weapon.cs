using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerCombatEffects combatEffects;

    void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();
        combatEffects = GetComponentInParent<PlayerCombatEffects>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (enemy != null)
        {
            float damage = stats.attackDamage;
            enemy.TakeDamage(damage);

            combatEffects?.OnDealDamage(damage, enemy);
        }
    }
}
