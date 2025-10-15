using System;
using UnityEngine;

[Serializable]
class UnlockProjectile : IUpgrade {
    [SerializeField] ProjectileAsset projectile;

    public UpgradeData GetUpgradeData() {
        return new UpgradeData {
            title = $"Unlock {projectile.ProjectileName}",
            description = "Unlock a new projectile type.",
            onUpgradeSelected = () => ProjectileRunner.Instance.InjectProjectile(projectile)
        };
    }
}