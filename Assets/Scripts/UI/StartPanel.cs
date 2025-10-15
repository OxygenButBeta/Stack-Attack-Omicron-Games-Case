using DG.Tweening;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(999)]
[RequireComponent(typeof(CanvasGroup))]
public class StartPanel : MonoBehaviour {
    [SerializeField] TMP_Text startText;
    public static bool ByRestart;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start() {
        Time.timeScale = 1;
        if (ByRestart) {
            ByRestart = false;
            PlayLevel();
            return;
        }

        startText.text = $"Play Level {GameConfigurationAsset.Default.CurrentLevelIndex + 1}";
    }
    void OnEnable() {
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.one * 0.8f;
        DOTween.Kill(this);
        Sequence seq = DOTween.Sequence().SetUpdate(UpdateType.Normal, true);
        seq.Append(canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad))
            .Join(rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        seq.SetTarget(this);
    }
    public void PlayLevel() {
        LevelRunner.Instance.ExecuteLevel(
            LevelDataMapper.GetMappedAssetAt(GameConfigurationAsset.Default.CurrentLevelIndex));
        gameObject.SetActive(false);
    }
}