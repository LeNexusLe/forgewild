using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatEffects : MonoBehaviour
{
    private PlayerStats stats;
    public float lifeStealPercent = 0f;
    public float burnDamagePerSecond = 0f;
    public float burnDuration = 0f;


    private Dictionary<IDamageable, float> activeBurnTimers = new();

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void AddLifeSteal(float amount)
    {
        lifeStealPercent += amount;
    }
    public void AddBurn(float damagePerSecond, float duration)
    {
        burnDamagePerSecond = damagePerSecond;
        burnDuration = duration;
    }


    public void OnDealDamage(float damage, IDamageable enemy)
    {
        if (lifeStealPercent > 0f)
        {
            stats.Heal(damage * lifeStealPercent);
        }

        if (burnDamagePerSecond > 0f && burnDuration > 0f && enemy != null)
        {    
            activeBurnTimers[enemy] = burnDuration;
        }
    }

    private void Update()
    {
        var targets = new List<IDamageable>(activeBurnTimers.Keys);

        foreach (var target in targets)
        {
            var mb = target as MonoBehaviour;
            if (mb == null)
            {
                activeBurnTimers.Remove(target);
                continue;
            }

            float timeLeft = activeBurnTimers[target];
            target.TakeDamage(burnDamagePerSecond * Time.deltaTime);
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0f)
                activeBurnTimers.Remove(target);
            else
                activeBurnTimers[target] = timeLeft;
        }
    }
}
