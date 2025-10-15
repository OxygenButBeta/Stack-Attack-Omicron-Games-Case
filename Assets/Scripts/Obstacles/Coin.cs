using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CircleCollider2D), typeof(VerticalFall))]
public class Coin : MonoBehaviour, ICollectable, ISelfPoolReleaseListener, ISelfPoolGetListener {
    [SerializeField] SFXAsset coinSFX;
    [Header("Tween Settings")] public float maxScale = 1.2f;
    public float tweenDuration = 0.5f;

    Tween loopTween;
    bool isCollected;
    float radius;
    bool beenInViewport;
    VerticalFall verticalFall;

    void Awake() {
        verticalFall = GetComponent<VerticalFall>();
        radius = GetComponent<CircleCollider2D>().radius;
    }

    public void OnCollect() {
        if (isCollected)
            return;

        Player.Instance.Score++;
        isCollected = true;
        ReferenceManager.Instance.AudioSource.PlayOneShot(coinSFX);

        Vector2 targetPosition =
            ViewportUtility.GetScreenTopLeftWorldPosition(ReferenceManager.Instance.CameraMain) * 1.2f;
        loopTween = transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InQuad);
    }


    void Update() {
        if (ViewportUtility.IsOutOfViewport(transform.position, radius, ReferenceManager.Instance.CameraMain)) {
            if (beenInViewport)
                StaticPool<Coin>.Release(this);
        }
        else
            beenInViewport = true;
    }

    public void OnSelfRelease() {
        SetSpeedModifier(1);
        loopTween?.Kill();
        isCollected = false;
        loopTween = null;
        transform.localScale = Vector3.one;
    }

    public void OnSelfGet() {
        transform.localScale = Vector3.one;

        loopTween = transform
            .DOScale(maxScale, tweenDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void SetSpeedModifier(float coinFallSpeedModifier) {
        verticalFall.SpeedModifier = coinFallSpeedModifier;
    }
}