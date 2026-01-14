using System.Collections.Generic;
using UnityEngine;

public class AutoShootEffect : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float range = 1f;
    public float fireRate = 0.2f;

    private float fireCooldown = 0f;

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            IDamageable target = FindNearestTarget();
            if (target != null)
            {
                ShootAt(target);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    private IDamageable FindNearestTarget()
    {
        var damageables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        IDamageable nearest = null;
        float closest = Mathf.Infinity;

        foreach (var mb in damageables)
        {
            if (mb is not IDamageable dmg) continue;

            float dist = Vector3.Distance(transform.position, mb.transform.position);
            if (dist < range && dist < closest)
            {
                closest = dist;
                nearest = dmg;
            }
        }

        return nearest;
    }

    private void ShootAt(IDamageable target)
    {
        if (projectilePrefab == null || target == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectilePlayer projectile = proj.GetComponent<ProjectilePlayer>();
        if (projectile != null)
        {
            if (target is MonoBehaviour mb)
            {
                projectile.SetTarget(mb.transform);
            }
        }
    }
}
