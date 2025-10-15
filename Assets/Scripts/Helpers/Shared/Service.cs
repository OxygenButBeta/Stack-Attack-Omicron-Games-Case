public static class Service<T> where T : class {
    static T instance;
    public static void SetInstance(T newInstance) {
        instance = newInstance;
    }
    public static T Instance {
        get {
            if (instance == null)
                throw new System.Exception($"Service<{typeof(T).Name}> not set");
            return instance;
        }
    }
    public static bool HasInstance => instance != null;
    public static void ClearInstance() => instance = null;
}