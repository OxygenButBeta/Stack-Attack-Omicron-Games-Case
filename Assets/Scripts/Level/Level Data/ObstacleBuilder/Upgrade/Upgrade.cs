using System;
using UnityEngine;

[Serializable]
public class Upgrade : ObstacleBuilder {
    [SerializeReference] IUpgrade[] upgrades;
    protected override bool ShowHorizontalIndex => false;
    protected override bool HasDelay => false;

    public override void Build(in ObstacleBuilderSettings settings) {
        UpgradeGUI.Instance.ShowUpgrades(upgrades);
    }
}