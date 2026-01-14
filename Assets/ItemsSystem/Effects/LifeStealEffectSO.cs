using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/LifeSteal")]
public class LifeStealEffectSO : ItemEffectSO
{
    [Range(0f, 1f)]
    public float lifeStealPercent = 0.1f;

    public override void Apply(GameObject player)
    {
        PlayerCombatEffects combatEffects = player.GetComponent<PlayerCombatEffects>();
        if (combatEffects == null)
            combatEffects = player.gameObject.AddComponent<PlayerCombatEffects>();

        combatEffects.AddLifeSteal(lifeStealPercent);
    }
}
