using UnityEngine;

public class HazardTile : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < lastDamageTime + damageCooldown)
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;

        playerHealth.TakeDamage(damage);
        lastDamageTime = Time.time;
    }
}
