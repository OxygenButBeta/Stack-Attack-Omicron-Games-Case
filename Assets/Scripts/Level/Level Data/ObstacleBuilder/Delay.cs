using System;

[Serializable]
public class Delay : ObstacleBuilder {
    protected override bool ShowHorizontalIndex => false;

    public override void Build(in ObstacleBuilderSettings settings) {
    }
}