using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Every scriptable that inherits from this class must have at least one ScriptableObject instance in the project.
// And This instance must be marked as `SingleAsset` addressable label in the inspector.
public class SingleScriptableAsset<T> : ScriptableObject, ISingleScriptableAsset where T : ScriptableObject {
    static T Asset;
    const string key = "SingleAsset";

    public static T Default {
        get {
            if (Asset == null)
                Asset = LoadAsset();

            return Asset;
        }
    }

    static T LoadAsset() {
        Debug.Log($"Loading single asset of type {typeof(T).Name} with key '{key}' from Addressables.");
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion();
        if (handle.Status == AsyncOperationStatus.Succeeded) {
            if (handle.Result is ISingleScriptableAsset single)
                single.OnLoadedAsSingle();

            return handle.Result;
        }

        Debug.LogError(
            $"Failed to load asset of type {typeof(T).Name}. Ensure that an instance is marked with 'SingleAsset' addressable label.");
        return null;
    }

    public virtual void OnLoadedAsSingle() {
    }
}