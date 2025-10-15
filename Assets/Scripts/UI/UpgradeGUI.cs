using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UpgradeGUI : Singleton<UpgradeGUI> {
    [SerializeField] Transform upgradeContainer;
    [SerializeField] UpgradeView upgradeViewPrefab;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    protected override void OnAwake() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void ShowUpgrades(IUpgrade[] upgrades) {
        foreach (Transform child in upgradeContainer) {
            Destroy(child.gameObject);
        }

        foreach (IUpgrade upgrade in upgrades) {
            UpgradeView upgradeView = Instantiate(upgradeViewPrefab, upgradeContainer);
            upgradeView.Initialize(upgrade, this);
            upgradeView.gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
        Time.timeScale = 0;
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

    public void OnUpgradeSelected(UpgradeData upgradeData) {
        upgradeData.onUpgradeSelected?.Invoke();
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}