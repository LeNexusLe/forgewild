using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;

    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public float attackDamage;
    public float attackCooldown;

    public float lifeStealPercent;
    public float burnDamagePerSecond;
    public float burnDuration;

    public int currentDay;
    public int currentBiomeIndex;

    public bool hasAutoShoot;
    public float autoShootFireRate;
    public float autoShootRange;
}
