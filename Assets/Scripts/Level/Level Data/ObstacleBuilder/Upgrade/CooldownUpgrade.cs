using System;
using UnityEngine;

[Serializable]
public class CooldownUpgrade : IUpgrade {
    [SerializeField] ProjectileAsset projectile;
    [SerializeField] float cooldownReduction;

    public UpgradeData GetUpgradeData() {
        return new UpgradeData {
            title = $"Reduce cooldown of {projectile.ProjectileName}",
            description = $"Reduce the fire rate of {projectile.ProjectileName} by {cooldownReduction} seconds.",
            onUpgradeSelected = () =>
                ProjectileRunner.Instance.OverrideProjectileCooldown(projectile, cooldownReduction)
        };
    }
}