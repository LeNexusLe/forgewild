using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        healthBar.UpdateHealthBar(stats.currentHP, stats.maxHP);
    }

    public void TakeDamage(float dmg)
    {
        stats.currentHP -= dmg;
        healthBar.UpdateHealthBar(stats.currentHP, stats.maxHP);
        if (stats.currentHP <= 0)
            MenuManager.instance.GameOver();
    }
}
