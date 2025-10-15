using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LevelCompletedPanel : MonoBehaviour {
    [SerializeField] Button nextLevelButton;
    [SerializeField] TMP_Text levelCompletedText;

    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
        LevelRunner.Instance.OnLevelOver += OnLevelCompleted;
        gameObject.SetActive(false);
    }

    void OnLevelCompleted(LevelData obj) {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    void OnEnable() {
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.one * 0.8f;
        DOTween.Kill(this);
        Sequence seq = DOTween.Sequence().SetUpdate(UpdateType.Normal, true);
        seq.Append(canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad))
            .Join(rectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
        seq.SetTarget(this);

        nextLevelButton.interactable =
            LevelDataMapper.Default.Assets.Count > GameConfigurationAsset.Default.CurrentLevelIndex + 1;
    }

    public void LoadNextLevel() {
        GameConfigurationAsset.Default.CurrentLevelIndex++;
        StartPanel.ByRestart = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void RestartLevel() {
        StartPanel.ByRestart = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}