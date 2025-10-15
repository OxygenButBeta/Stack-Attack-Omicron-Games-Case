using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VFXPoolMap = AssetPoolMap<VFXPairAssetKey, VFXPair>;

[DefaultExecutionOrder(-100)]
public class PoolInitializer : Singleton<PoolInitializer> {
    [SerializeField] Transform poolHolderTransform;

    [ReadOnly, ShowInInspector, TabGroup("Instance Data")]
    int totalCellsEverCreated;

    [ReadOnly, ShowInInspector, TabGroup("Instance Data")]
    int totalStacksEverCreated;

    [ReadOnly, ShowInInspector, TabGroup("Instance Data")]
    int totalVfxEverCreated;


    protected override void OnAwake() {
        InitilalizeStaticPools();
        InitializeVFXPool();
    }

    void InitializeVFXPool() {
        var values = (VFXPairAssetKey[])Enum.GetValues(typeof(VFXPairAssetKey));
        VFXPoolMap vfxPool =
            PoolUtilities.BuildPoolsFromScriptableAssetsMap(
                values,
                createFunc: asset => {
                    totalVfxEverCreated++;
                    VFXPair vfx = Instantiate(VFXPairMapper.GetMappedAsset(asset));
                    vfx.gameObject.SetActive(false);
                    return vfx;
                },
                actionOnGet: vfx => vfx.gameObject.SetActive(true),
                actionOnRelease: (vfx) => {
                    vfx.gameObject.SetActive(false);
                    vfx.transform.SetParent(poolHolderTransform, true);
                });

        Service<VFXPoolMap>.SetInstance(vfxPool);
    }

    void InitilalizeStaticPools() {
        // Cell Pool
        StaticPool<Cell>.Initialize(() => {
                totalCellsEverCreated++;
                Cell cell = Instantiate(ReferenceManager.Instance.CellPrefab);
                cell.gameObject.SetActive(false);
                return cell;
            }, actionOnGet: (cell) => {
                cell.transform.localScale = Vector3.one;
                cell.gameObject.SetActive(true);
            },
            actionOnRelease: (cell) => {
                cell.gameObject.SetActive(false);
                cell.transform.SetParent(poolHolderTransform, true);
            },
            collectionCheck: true,
            defaultCapacity: 100);

        // Stack pool
        StaticPool<VerticalCellStack>.Initialize(() => {
                totalStacksEverCreated++;
                VerticalCellStack cell = Instantiate(ReferenceManager.Instance.VerticalStackPrefab);
                cell.gameObject.SetActive(false);
                return cell;
            }, actionOnGet: (cell) => {
                cell.transform.SetParent(null);
                cell.gameObject.SetActive(true);
            },
            actionOnRelease: (cell) => {
                cell.gameObject.SetActive(false);
                cell.transform.SetParent(poolHolderTransform, true);
                cell.transform.position = ViewportUtility.GetTopCenterPosition(Camera.main);
            },
            collectionCheck: false,
            defaultCapacity: 100);
        StaticPool<Dynamite>.Initialize(() => {
                Dynamite dynamite = Instantiate(ReferenceManager.Instance.DynamitePrefab);
                dynamite.gameObject.SetActive(false);
                return dynamite;
            }, actionOnGet: (explosive) => {
                explosive.transform.localScale = Vector3.one;
                explosive.gameObject.SetActive(true);
            },
            actionOnRelease: (explosive) => {
                explosive.gameObject.SetActive(false);
                explosive.transform.SetParent(poolHolderTransform, true);
            },
            collectionCheck: true,
            defaultCapacity: 20);

        StaticPool<Coin>.Initialize(() => {
                Coin coin = Instantiate(ReferenceManager.Instance.CoinPrefab);
                coin.gameObject.SetActive(false);
                return coin;
            }, actionOnGet: (coin) => {
                coin.transform.localScale = Vector3.one;
                coin.gameObject.SetActive(true);
            },
            actionOnRelease: (coin) => {
                coin.gameObject.SetActive(false);
                coin.transform.SetParent(poolHolderTransform, true);
            },
            collectionCheck: true,
            defaultCapacity: 100);
    }
}