using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FailScreen : MonoBehaviour {
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
        Player.Instance.OnPlayerDead += ShowFailScreen;
        gameObject.SetActive(false);
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

    void ShowFailScreen() {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void RestartLevel() {
        StartPanel.ByRestart = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ExitToMenu() {
        Application.Quit();
    }
}