using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>{
    static T _instance;

    public static T Instance{
        get
        {
            if (_instance)
                return _instance;

            _instance = FindAnyObjectByType<T>();
            return _instance;
        }
    }

    protected void Awake(){
        if (_instance != null && _instance != this){
            Debug.LogWarning($"[Singleton] Duplicate instance of {typeof(T)} detected, destroying new one.");
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
        OnAwake();
    }

    protected virtual void OnAwake(){
    }
}