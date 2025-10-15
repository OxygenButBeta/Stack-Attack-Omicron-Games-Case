using System;
using UnityEngine;

[Serializable]
public class AddHealthUpgrade : IUpgrade {
    [SerializeField] int healthIncrease;
    public UpgradeData GetUpgradeData() {
        return new UpgradeData {
            title = "Increase Health",
            description = $"Increase player health by {healthIncrease}.",
            onUpgradeSelected = () => Player.Instance.Health += healthIncrease
        };
    }
}