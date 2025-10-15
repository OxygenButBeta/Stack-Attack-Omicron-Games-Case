using System;
using UnityEngine;

[Serializable]
public class DynamiteBuilder : ObstacleBuilder {
    [SerializeField] Dynamite.ExplosiveData explosiveData;

    public override void Build(in ObstacleBuilderSettings settings) {
        Dynamite dynamite = StaticPool<Dynamite>.Get();
        dynamite.transform.position = GetSpawnPosition(in settings);
        dynamite.SetExplosiveData(explosiveData);
    }
}