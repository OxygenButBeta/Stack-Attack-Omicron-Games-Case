using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject {
    [TabGroup("Shared Builder Settings"), InlineProperty]
    public ObstacleBuilderSettings BuilderSettings;

    [field: SerializeField] public ProjectileAssetBind[] StartProjectiles { get; private set; }
    [field: SerializeField] public int InitialPlayerHealth { get; private set; } = 3;

    [ListDrawerSettings(
        DraggableItems = true,
        ShowPaging = false,
        ShowFoldout = true,
        DefaultExpandedState = true
    )]
    [SerializeReference]
    public ObstacleBuilder[] Obstacles = Array.Empty<ObstacleBuilder>();

#if UNITY_EDITOR
    [ShowIf(nameof(IsPlaying))]
    [Button(ButtonSizes.Large), GUIColor(0.3f, 0.8f, 1f)]
    void TestInPlayMode() {
        LevelRunner.Instance.ExecuteLevel(this);
    }

    bool IsPlaying => Application.isPlaying;
#endif
}