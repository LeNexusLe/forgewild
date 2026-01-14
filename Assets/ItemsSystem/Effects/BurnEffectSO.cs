using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/Burn")]
public class BurnlEffectSO : ItemEffectSO
{
    [Range(0f, 1f)]
    public float burnDamagePerSecond = 1f;
    public float burnDuration = 1f;

    public override void Apply(GameObject player)
    {
        PlayerCombatEffects combatEffects = player.GetComponent<PlayerCombatEffects>();
        if (combatEffects == null)
            combatEffects = player.gameObject.AddComponent<PlayerCombatEffects>();

        combatEffects.AddBurn(burnDamagePerSecond, burnDuration);
    }
}
