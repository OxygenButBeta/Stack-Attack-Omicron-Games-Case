using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[Serializable]
public class StackShakeEffect : IDamageEffect {
    [SerializeField] float punchAmount = 0.1f;
    [SerializeField] float duration = 0.3f;

    Sequence shakeSequence;

    public bool IsPlaying => shakeSequence.IsActive();

    public void ExecuteEffect(ICellStack stack) {
        shakeSequence = DOTween.Sequence();
        Vector3 originalScale = stack.Transform.localScale;
        shakeSequence.Append(stack.Transform
                .DOPunchScale(new Vector3(punchAmount, -punchAmount, 0), duration, vibrato: 10, elasticity: 1f)
                .SetEase(Ease.OutElastic))
            .OnComplete(() => CleanUpAnimation(stack.Transform, originalScale));
        shakeSequence.onKill += () => CleanUpAnimation(stack.Transform, originalScale);
    }

    void CleanUpAnimation(Transform transform, Vector3 originalScale) {
        transform.localScale = originalScale;
    }

    public void StopImmediate() {
        shakeSequence?.Kill();
    }
}