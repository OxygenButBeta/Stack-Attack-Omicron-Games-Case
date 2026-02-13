using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>{
    static T instance;
    public static T Instance{
        get
        {
            if (instance)
                return instance;

            instance = FindAnyObjectByType<T>();
            return instance;
        }
    }
    protected void Awake(){
        if (instance && instance != this){
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} detected, destroying new one.");
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
        OnAwake();
    }
    protected virtual void OnAwake(){
    }
}