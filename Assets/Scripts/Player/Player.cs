using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Singleton<Player> {
    [SerializeField, ReadOnly] int health = 3;
    [SerializeField, ReadOnly] int score = 0;
    public event Action<int> OnHealthChanged;
    public event Action OnPlayerTookDamage;

    public event Action<int> OnScoreChanged;
    public event Action OnPlayerDead;

    [SerializeField] SFXAsset damageSFX;

    public int Score {
        get => score;
        set {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }

    public int Health {
        get => health;
        set {
            health = value;
            OnHealthChanged?.Invoke(health);
            if (health == 0)
                OnPlayerDead?.Invoke();
        }
    }



    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Obstacle"))
            return;
        if (!other.TryGetComponent(out ICollectable collectable))
            return;
        collectable.OnCollect();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.TryGetComponent(out IPlayerDamager damager))
            return;
        ReferenceManager.Instance.AudioSource.PlayOneShot(damageSFX);
        OnPlayerTookDamage?.Invoke();
        Health = Math.Max(0, Health - damager.OnCollideWithPlayer());
    }
}