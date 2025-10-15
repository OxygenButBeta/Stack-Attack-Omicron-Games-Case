using System;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class VerticalStackBuilder : ObstacleBuilder {
    [SerializeField] StackAssetBind StackAsset;

    public override void Build(in ObstacleBuilderSettings settings) {
        VerticalCellStack vStack = StaticPool<VerticalCellStack>.Get();
        vStack.SetStackAsset(StackAsset);
        vStack.transform.position = GetSpawnPosition(in settings);
    }
}