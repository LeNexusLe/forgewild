using UnityEngine;

public enum Rarity
{
    Common,
    Epic,
    Legendary
}


[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Header("Drop settings")]
    public float weight = 50f;
    public Rarity rarity;

    [Header("Stat boosts")]
    public StatModifier[] statModifiers;

    [Header("Special effects")]
    public ItemEffectSO[] specialEffects;
}
