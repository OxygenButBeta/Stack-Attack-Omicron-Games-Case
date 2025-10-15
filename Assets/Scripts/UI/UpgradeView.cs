using TMPro;
using UnityEngine;

public class UpgradeView : MonoBehaviour {
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    UpgradeData upgradeData;
    UpgradeGUI upgradeGUI;

    public void SelectUpgrade() {
        upgradeGUI.OnUpgradeSelected(upgradeData);
    }

    public void Initialize(IUpgrade upgrade, UpgradeGUI upgrader) {
        upgradeGUI = upgrader;
        upgradeData = upgrade.GetUpgradeData();
        titleText.text = upgradeData.title;
        descriptionText.text = upgradeData.description;
    }
}