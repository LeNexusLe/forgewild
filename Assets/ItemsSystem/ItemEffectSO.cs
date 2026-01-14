using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public string displayName;
    public bool isPercent = true;

    public abstract void Apply(GameObject player);
}
