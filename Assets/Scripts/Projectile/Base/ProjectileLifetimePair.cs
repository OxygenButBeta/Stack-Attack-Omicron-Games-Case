using System;
using Sirenix.OdinInspector;

[Serializable]
internal class ProjectileLifetimePair {
    [ReadOnly, ShowInInspector] float timer;
    public ProjectileAsset projectileAsset;
    public float Cooldown => projectileAsset.ProjectileCooldown;

    public void Tick(float deltaTime) {
        if (timer > 0f)
            timer -= deltaTime;
    }

    public bool CanLaunch(float clamp = 0) => timer - clamp <= 0f;

    public void ResetTimer() {
        timer = Cooldown;
    }
}