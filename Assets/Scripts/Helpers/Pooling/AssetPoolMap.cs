using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AssetPoolMap<TScriptable, TMono> where TMono : class {
    readonly Dictionary<TScriptable, ObjectPool<TMono>> poolDictionary;

    public AssetPoolMap(Dictionary<TScriptable, ObjectPool<TMono>> pool) {
        poolDictionary = pool;
    }

    public TMono Get(TScriptable asset) {
        if (asset == null) {
            Debug.LogError("Asset is null — cannot get from pool.");
            return null;
        }

        if (poolDictionary.TryGetValue(asset, out var pool))
            return pool.Get();

        Debug.LogError($"No pool found for asset: {asset}");
        return null;
    }

    public void Release(TScriptable asset, TMono instance) {
        if (asset is null || instance is null)
            return;

        if (poolDictionary.TryGetValue(asset, out ObjectPool<TMono> pool))
            pool.Release(instance);
        else
            Debug.LogWarning($"Trying to release instance from a pool that doesn't exist for asset: {asset}");
    }
}