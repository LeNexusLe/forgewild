using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void AddItem(Item item)
    {
        foreach (StatModifier mod in item.statModifiers)
            mod.Apply(stats);

        foreach (ItemEffectSO effect in item.specialEffects)
            effect.Apply(gameObject);

        ItemPickupUIController.Instance.ShowItemPickup(item.itemName, item.icon, item.rarity);
    }
}
