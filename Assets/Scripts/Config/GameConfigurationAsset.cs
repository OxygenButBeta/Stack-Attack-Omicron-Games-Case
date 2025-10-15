using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/Game Configuration Asset")]
public class GameConfigurationAsset : SingleScriptableAsset<GameConfigurationAsset> {
    [field: SerializeField] public float Sensitivity { get; private set; } = 1f;
    [field: SerializeField] public float FallingSpeed { get; private set; } = 1f;
    [field: SerializeField] public Vector2Int ScreenPosToGridResolution { get; private set; } = new(10, 20);
    [field: SerializeField] public float PlayerCollideDamage { get; private set; } = 1f;

    int currentLevelIndex;

    const string CurrentLevelKey = "CurrentLevelIndex";

    public int CurrentLevelIndex {
        get => currentLevelIndex;
        set {
            currentLevelIndex = value;
            PlayerPrefs.SetInt(CurrentLevelKey, currentLevelIndex);
        }
    }

    [Button]
    void ClearLevelProgress() {
        PlayerPrefs.DeleteKey(CurrentLevelKey);
        CurrentLevelIndex = 0;
    }

    public override void OnLoadedAsSingle() {
        CurrentLevelIndex = PlayerPrefs.GetInt(CurrentLevelKey, 0);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void TweenConfig() {
        DG.Tweening.DOTween.Init(false, true, DG.Tweening.LogBehaviour.ErrorsOnly);
    }
}