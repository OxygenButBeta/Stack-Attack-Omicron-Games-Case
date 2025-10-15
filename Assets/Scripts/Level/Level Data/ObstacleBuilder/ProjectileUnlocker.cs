using System;
using UnityEngine;

[Serializable]
public class ProjectileUnlocker : ObstacleBuilder {
    [SerializeField] ProjectileAssetBind projectile;
    protected override bool ShowHorizontalIndex => false;

    public override void Build(in ObstacleBuilderSettings settings) {
        ProjectileRunner.Instance.InjectProjectile(projectile);
    }
}