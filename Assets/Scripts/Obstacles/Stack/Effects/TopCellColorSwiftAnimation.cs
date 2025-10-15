using DG.Tweening;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[System.Serializable]
public class TopCellColorSwiftAnimation : IDamageEffect {
    [SerializeField] Color targetColor;
    [SerializeField] LoopType loopType = LoopType.Yoyo;
    [SerializeField] float duration = 0.3f;
    [SerializeField] int loops = 2;
    Sequence colorSequence;
    public bool IsPlaying => colorSequence.IsActive();
    public void ExecuteEffect(ICellStack stack) {
        Cell topCell = stack.GetLastCell();
        if (topCell == null)
            return;

        colorSequence = DOTween.Sequence();
        Color originalColor = topCell.spriteComponent.color;
        SpriteRenderer sprite = topCell.spriteComponent;

        colorSequence.Join(sprite.DOColor(targetColor, duration / 2)
            .SetLoops(loops, loopType));
        colorSequence.onKill += () => sprite.color = originalColor;
        colorSequence.onComplete += () => sprite.color = originalColor;
    }

    public void StopImmediate() {
        colorSequence?.Kill();
    }
}