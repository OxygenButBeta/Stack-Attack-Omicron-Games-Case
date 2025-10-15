using System;
using UnityEngine.Pool;

public static class StaticPool<TObject> where TObject : class {
    static ObjectPool<TObject> pool;

    // ReSharper disable once StaticMemberInGenericType
    static bool isInitialized;

    public static void Initialize(Func<TObject> createFunc, Action<TObject> actionOnGet = null,
        Action<TObject> actionOnRelease = null,
        Action<TObject> actionOnDestroy = null,
        bool collectionCheck = true,
        int defaultCapacity = 10,
        int maxSize = 10000) {
        pool = new ObjectPool<TObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck,
            defaultCapacity, maxSize);
        isInitialized = true;
    }

    public static TObject Get() {
        if (!isInitialized)
            throw new Exception("Pool not initialized");

        TObject v = pool.Get();

        if (v is ISelfPoolGetListener listener)
            listener.OnSelfGet();

        return v;
    }

    public static PooledObject<TObject> Get(out TObject v) {
        if (!isInitialized)
            throw new Exception("Pool not initialized");

        PooledObject<TObject> p = pool.Get(out v);

        if (v is ISelfPoolGetListener listener)
            listener.OnSelfGet();

        return p;
    }

    public static void Release(TObject element) {
        if (!isInitialized)
            throw new Exception("Pool not initialized");

        pool.Release(element);
        if (element is ISelfPoolReleaseListener listener)
            listener.OnSelfRelease();
    }

    public static void Clear() {
        if (!isInitialized)
            throw new Exception("Pool not initialized");

        pool.Clear();
    }

    public static int CountInactive {
        get {
            if (!isInitialized)
                throw new Exception("Pool not initialized");

            return pool.CountInactive;
        }
    }
}