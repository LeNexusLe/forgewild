using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float maxHP = 10f;
    public float weight = 50f;
    public int startDay = 1; // Od kiedy sie pojawia

    private float currentHP;

    public float CurrentHP => currentHP;

    [SerializeField] HealthBar healthBar;
    private IEnemyAttack attack;

    private void Awake()
    {
        currentHP = maxHP;
        attack = GetComponent<IEnemyAttack>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(currentHP, maxHP);
    }

    private void Update()
    {
        attack?.TryAttack();
    }


    public void TakeDamage(float dmg)
    {
        if (currentHP <= 0) return;

        currentHP -= dmg;
        healthBar.UpdateHealthBar(currentHP, maxHP);
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
