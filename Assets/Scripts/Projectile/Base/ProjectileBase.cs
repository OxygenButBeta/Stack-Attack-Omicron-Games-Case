using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class ProjectileBase : MonoBehaviour {
    [SerializeField] protected SFXAsset launchSFX;
    
    [SerializeField] protected float Damage = 1f;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] Collider2D col2D;
    [SerializeField] protected float speed = 10f;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] VFXPairAssetKey vfxHitKey;

    protected Action<ProjectileBase, IDamageReceiver> onHitCallback;
    protected Action<ProjectileBase> onOutOfViewportCallback;
    public void SetOnHitCallback(Action<ProjectileBase, IDamageReceiver> callback) => onHitCallback = callback;
    public void SetOnOutOfViewportCallback(Action<ProjectileBase> callback) => onOutOfViewportCallback = callback;

    public abstract void Launch(Vector3 targetDirection);
    protected abstract bool OnHitDamageReceiver(IDamageReceiver damageReceiver);

    protected virtual void Update() {
        CheckViewport();
    }

    void CheckViewport() {
        if (ViewportUtility.IsOutOfViewport(col2D, ReferenceManager.Instance.CameraMain))
            onOutOfViewportCallback?.Invoke(this);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (((1 << other.gameObject.layer) & TargetLayer) == 0)
            return;


        if (!other.gameObject.TryGetComponent(out IDamageReceiver damageReceiver))
            return;

        if (!OnHitDamageReceiver(damageReceiver))
            return;

        ReferenceManager.VFXPoolMap.Get(vfxHitKey).SetPosition(transform.position);
        onHitCallback?.Invoke(this, damageReceiver);
    }

    public virtual void ResetProjectile() {
        rb2D.linearVelocity = Vector2.zero;
        rb2D.angularVelocity = 0f;
    }
}