using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class PoolUtilities {
    public delegate TMono CreateFromAssetFunc<in TAsset, out TMono>(TAsset asset)
        where TMono : class ;

    public static AssetPoolMap<TScriptable, TMono> BuildPoolsFromScriptableAssetsMap<TScriptable, TMono>(
        IEnumerable<TScriptable> scriptableAssets,
        CreateFromAssetFunc<TScriptable, TMono> createFunc,
        Action<TMono> actionOnGet = null,
        Action<TMono> actionOnRelease = null,
        Action<TMono> actionOnDestroy = null,
        bool collectionCheck = true,
        int defaultCapacity = 10,
        int maxSize = 10000)
        where TMono : class
    {
        Dictionary<TScriptable, ObjectPool<TMono>> dict = BuildPoolsFromScriptableAssets(scriptableAssets, createFunc,
            actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);

        return new AssetPoolMap<TScriptable, TMono>(dict);
    }

    public static Dictionary<TScriptable, ObjectPool<TMono>>
        BuildPoolsFromScriptableAssets<TScriptable, TMono>(IEnumerable<TScriptable> scriptableAssets,
            CreateFromAssetFunc<TScriptable, TMono> createFunc,
            Action<TMono> actionOnGet = null,
            Action<TMono> actionOnRelease = null,
            Action<TMono> actionOnDestroy = null,
            bool collectionCheck = true,
            int defaultCapacity = 10,
            int maxSize = 10000) where TMono : class{
        Dictionary<TScriptable, ObjectPool<TMono>> pools = new();
        foreach (TScriptable scriptableAsset in scriptableAssets) {
            ObjectPool<TMono> pool = BuildPoolForScriptable(scriptableAsset, createFunc, actionOnGet, actionOnRelease,
                actionOnDestroy,
                collectionCheck, defaultCapacity, maxSize);
            pools.Add(scriptableAsset, pool);
        }

        return pools;
    }

    static ObjectPool<TMono> BuildPoolForScriptable<TMono, TScriptable>(TScriptable asset,
        CreateFromAssetFunc<TScriptable, TMono> createFunc,
        Action<TMono> actionOnGet, Action<TMono> actionOnRelease, Action<TMono> actionOnDestroy, bool collectionCheck,
        int defaultCapacity, int maxSize) where TMono : class {
        return new ObjectPool<TMono>(() => createFunc(asset), actionOnGet, actionOnRelease, actionOnDestroy,
            collectionCheck,
            defaultCapacity, maxSize);
    }
}