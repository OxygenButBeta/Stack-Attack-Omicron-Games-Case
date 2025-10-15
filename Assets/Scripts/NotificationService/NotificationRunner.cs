using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class NotificationRunner : Singleton<NotificationRunner> {
    [Required, SerializeField] TMP_Text textComponent;
    [Required, SerializeField] TMP_Text shadowTextComponent;
    [Required, SerializeField] RectTransform textParentTransform;
    Sequence sequence;

    protected override void OnAwake() {
        textParentTransform.gameObject.SetActive(false);
    }

    public void Push(string msg, float duration = 2f) {
        if (sequence != null && sequence.IsActive())
            sequence.Kill();

        textComponent.text = msg;
        shadowTextComponent.text = msg;

        textParentTransform.gameObject.SetActive(true);
        textParentTransform.anchoredPosition = new Vector2(-1328, 0);

        sequence = DOTween.Sequence();

        var enterTime = duration * 0.25f;  
        var stayTime = duration * 0.5f;  
        var exitTime = duration * 0.25f;  
        const float endX = 1500f;

        sequence.Append(textParentTransform.DOAnchorPosX(0, enterTime)
            .SetEase(Ease.OutCubic));

        sequence.AppendInterval(stayTime);

        sequence.Append(textParentTransform.DOAnchorPosX(endX, exitTime)
            .SetEase(Ease.InCubic));

        sequence.OnComplete(() =>
        {
            textParentTransform.gameObject.SetActive(false);
            textParentTransform.anchoredPosition = Vector2.zero;
        });
    }
}