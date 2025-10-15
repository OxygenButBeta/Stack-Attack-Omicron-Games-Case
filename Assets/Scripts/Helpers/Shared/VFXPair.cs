using System;
using System.Collections;
using UnityEngine;

public class VFXPair : MonoBehaviour {
    [SerializeField] float duration;
    [SerializeField] VFXPairAssetKey vfxKey;

    Action beforeReleaseCallback;

    void OnEnable() {
        StartCoroutine(Release());
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    public void SetPosition(Vector3 position) {
        position.z = 10f;
        transform.position = position;
    }

    IEnumerator Release() {
        yield return new WaitForSeconds(duration);
        beforeReleaseCallback?.Invoke();
        beforeReleaseCallback = null;
        ReferenceManager.VFXPoolMap.Release(vfxKey, this);
    }

    public void SetBeforeReleaseCallback(Action callback) {
        beforeReleaseCallback = callback;
    }
}