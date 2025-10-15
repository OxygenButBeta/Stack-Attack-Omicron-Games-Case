using System;
using UnityEngine;

[Serializable]
public class GridBuilder : ObstacleBuilder {
    [SerializeField] int rowCount = 4;
    [SerializeField] float rowSpacing = 2f;
    [SerializeField] int columnCount = 4;
    [SerializeField] float startXOffset = .2f;
    [SerializeField] float columnSpacing = .7f;
    [SerializeField] StackAssetBind[] AvailableStacks;

    protected override bool ShowHorizontalIndex => false;

    public override void Build(in ObstacleBuilderSettings settings) {
        for (var y = 0; y < rowCount; y++)
        for (var x = 0; x < columnCount; x++) {
            StackAssetBind StackAsset = AvailableStacks[UnityEngine.Random.Range(0, AvailableStacks.Length)];
            VerticalCellStack stackInstance = StaticPool<VerticalCellStack>.Get();
            stackInstance.SetStackAsset(StackAsset);
            Vector3 pos = GetSpawnPosition(in settings, x);
            stackInstance.transform.position =
                new Vector3(pos.x + startXOffset + (x * columnSpacing), pos.y + (y * rowSpacing), 0);
        }
    }
}