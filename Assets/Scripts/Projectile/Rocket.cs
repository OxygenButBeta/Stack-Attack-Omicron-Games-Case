using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : ProjectileBase {
    [SerializeField] float rotForce;
    [SerializeField] TrailRenderer trail;
    [SerializeField] AnimationCurve[] curves;
    [Range(0, 1)] [SerializeField] float verticalClip = .15f;

    float time;
    float direction;
    AnimationCurve currentCurve;

    public override void Launch(Vector3 targetDirection) {
        ReferenceManager.Instance.AudioSource.PlayOneShot(launchSFX);
        time = 0;
        rb2D.linearVelocity = targetDirection.normalized * speed;
        currentCurve = curves[Random.Range(0, curves.Length)];
        direction = Random.Range(0, 100) % 2 == 0 ? 1 : -1;
        direction *= verticalClip;
    }

    protected override void Update() {
        base.Update();
        time += Time.deltaTime;
        var actualVelocity = currentCurve.Evaluate(time) * rotForce * direction;
        rb2D.AddForce(actualVelocity * Vector3.right, ForceMode2D.Force);
    }

    public override void ResetProjectile() {
        base.ResetProjectile();
        trail.Clear();
    }

    protected override bool OnHitDamageReceiver(IDamageReceiver damageReceiver) {
        damageReceiver.OnDamage(Damage);
        return true;
    }
}