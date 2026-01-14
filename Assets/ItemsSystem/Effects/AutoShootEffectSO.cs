using UnityEngine;

[CreateAssetMenu(menuName = "Items/Effects/AutoShoot")]
public class AutoShootEffectSO : ItemEffectSO
{
    public GameObject projectilePrefab;
    public float range = 1f;
    public float fireRate = 0.3f;

    public override void Apply(GameObject player)
    {
        AutoShootEffect autoShoot = player.GetComponent<AutoShootEffect>();
        if (autoShoot == null)
            autoShoot = player.AddComponent<AutoShootEffect>();

        autoShoot.projectilePrefab = projectilePrefab;
        autoShoot.range += range;
        autoShoot.fireRate += fireRate;
    }
}
