using UnityEngine;

public class EnemyAttack_Melee : MonoBehaviour
{
    public int damage = 2;
    public float cooldown = 1f;

    private float lastAttackTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < lastAttackTime + cooldown) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;

        health.TakeDamage(damage);
        lastAttackTime = Time.time;
    }
}
