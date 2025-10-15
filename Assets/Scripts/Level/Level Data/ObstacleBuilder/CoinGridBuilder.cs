using UnityEngine;

[System.Serializable]
public class CoinGridBuilder : ObstacleBuilder {
    [SerializeField] int rowCount = 4;
    [SerializeField] float rowSpacing = 2f;
    [SerializeField] int columnCount = 4;
    [SerializeField] float startXOffset = .2f;
    [SerializeField] float columnSpacing = .7f;
    [SerializeField] float coinFallSpeedModifier = 1f;

    protected override bool ShowHorizontalIndex => false;

    public override void Build(in ObstacleBuilderSettings settings) {
        for (var y = 0; y < rowCount; y++)
        for (var x = 0; x < columnCount; x++) {
            Coin coinInstance = StaticPool<Coin>.Get();
            coinInstance.SetSpeedModifier(coinFallSpeedModifier);
            Vector3 pos = GetSpawnPosition(in settings, x);
            coinInstance.transform.position =
                new Vector3(pos.x + startXOffset + (x * columnSpacing), pos.y + (y * rowSpacing), 0);
        }
    }
}