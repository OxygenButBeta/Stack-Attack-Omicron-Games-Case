using Sirenix.OdinInspector;
using UnityEngine;

public class Ball : ProjectileBase {
    [Required, SerializeField] TrailRenderer trail;

    public override void Launch(Vector3 targetDirection) {
        ReferenceManager.Instance.AudioSource.PlayOneShot(launchSFX);
        rb2D.linearVelocity = targetDirection.normalized * speed;
    }

    [Button]
    void TestLaunch(Vector3 targetPosition) {
        Launch(targetPosition);
    }

    protected override bool OnHitDamageReceiver(IDamageReceiver damageReceiver) {
        damageReceiver.OnDamage(Damage);
        return true;
    }

    public override void ResetProjectile() {
        base.ResetProjectile();
        trail.Clear();
    }
}