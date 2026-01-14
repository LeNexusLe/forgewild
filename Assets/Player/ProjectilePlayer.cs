using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float damage = 1;
    public float lifeTime = 3f;
    public float speed = 5f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
