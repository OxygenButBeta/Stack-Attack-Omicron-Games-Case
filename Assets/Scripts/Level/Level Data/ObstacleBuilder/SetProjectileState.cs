using System;
using UnityEngine;

[Serializable]
public class SetProjectileState : ObstacleBuilder {
    [SerializeField] bool state = true;
    protected override bool ShowHorizontalIndex => false;
    protected override bool HasDelay => false;

    public override void Build(in ObstacleBuilderSettings settings) {
        ProjectileRunner.Instance.enabled = state;
    }
}