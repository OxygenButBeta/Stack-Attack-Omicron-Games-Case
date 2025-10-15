using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Dynamite : MonoBehaviour, IDamageReceiver, ISelfPoolGetListener, IExplosive {
    readonly Collider2D[] hitColliders = new Collider2D[10];
    [SerializeField] VFXPairAssetKey vfxExplosionKey;
    ExplosiveData explosiveData;
    bool exploded;

    [System.Serializable]
    public struct ExplosiveData {
        public float radius;
        public float damage;
        public bool triggerOtherExplosives;
    }

    public void SetExplosiveData(ExplosiveData data) {
        explosiveData = data;
    }

    public void OnDamage(float damage) {
        if (exploded)
            return;

        exploded = true;
#pragma warning disable CS0618 // Type or member is obsolete
        var result = Physics2D.OverlapCircleNonAlloc(transform.position, explosiveData.radius, hitColliders);
#pragma warning restore CS0618 // Type or member is obsolete
        for (var i = 0; i < result; i++) {
            Collider2D hitCollider = hitColliders[i];
            if (!hitCollider.TryGetComponent(out IDamageReceiver damageReceiver))
                continue;
            
            if (!explosiveData.triggerOtherExplosives && hitCollider.TryGetComponent(out IExplosive _))
                continue;

            damageReceiver.OnDamage(explosiveData.damage);
        }
        ReferenceManager.VFXPoolMap.Get(vfxExplosionKey).SetPosition( transform.position);
        StaticPool<Dynamite>.Release(this);
    }

    public void OnSelfGet() {
        exploded = false;
    }
}

// Marker interface for explosives to trigger chain reactions
public interface IExplosive {
}