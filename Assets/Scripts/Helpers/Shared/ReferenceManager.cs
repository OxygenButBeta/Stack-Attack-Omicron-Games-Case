using Sirenix.OdinInspector;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ReferenceManager : Singleton<ReferenceManager> {
    [field: SerializeField, ReadOnly] public Camera CameraMain { get; private set; }

    [TabGroup("Prefabs"), Required]
    [field: SerializeField]
    public Cell CellPrefab { get; private set; }

    [TabGroup("Prefabs"), Required]
    [field: SerializeField]
    public Coin CoinPrefab { get; private set; }

    [TabGroup("Prefabs"), Required]
    [field: SerializeField]
    public VerticalCellStack VerticalStackPrefab { get; private set; }

    [TabGroup("Prefabs"), Required]
    [field: SerializeField]
    public Dynamite DynamitePrefab { get; private set; }

    [TabGroup("Prefabs"), Required]
    [field: SerializeField]
    public Boss BossPrefab { get; private set; }
    

    [TabGroup("Sys"), Required]
    [field: SerializeField]
    public AudioSource AudioSource { get; private set; }


    public static AssetPoolMap<VFXPairAssetKey, VFXPair> VFXPoolMap =>
        Service<AssetPoolMap<VFXPairAssetKey, VFXPair>>.Instance;

    protected override void OnAwake() {
        CameraMain = Camera.main;
    }
}