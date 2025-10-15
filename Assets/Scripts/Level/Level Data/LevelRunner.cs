using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelRunner : Singleton<LevelRunner> {
    List<ObstacleBuilder> builded;

    public event Action<LevelData> OnLevelOver;

    public event Action<LevelData> OnLevelStarted;

    void Start() {
        Player.Instance.OnPlayerDead += () => {
            if (builded != null) {
                foreach (ObstacleBuilder builder in builded)
                    builder.Cleanup();
            }

            builded = null;
            StopAllCoroutines();
        };
    }

    [Button]
    public void ExecuteLevel(LevelData levelData) {
        if (!levelData) {
            Debug.LogError("LevelData is null.");
            return;
        }

        if (builded != null) {
            Debug.LogWarning("A level is already running.");
            return;
        }

        OnLevelStarted?.Invoke(levelData);


        builded = new List<ObstacleBuilder>(capacity: levelData.Obstacles.Length);
        Player.Instance.Health = levelData.InitialPlayerHealth;
        if (levelData.StartProjectiles.Length == 0)
            Debug.LogWarning("No starting projectiles defined for this level.");

        ProjectileRunner.Instance.Clear();
        foreach (ProjectileAssetBind projectileAsset in levelData.StartProjectiles)
            ProjectileRunner.Instance.InjectProjectile(projectileAsset);


        StartCoroutine(ExecuteLevelRoutine(levelData));
    }

    void Update() {
        if (builded == null)
            return;

        foreach (ObstacleBuilder obstacleBuilder in builded)
            obstacleBuilder.Update();
    }

    IEnumerator ExecuteLevelRoutine(LevelData levelData) {
        foreach (ObstacleBuilder obstacleBuilder in levelData.Obstacles) {
            yield return new WaitForSeconds(obstacleBuilder.StartDelay);
            obstacleBuilder.Build(in levelData.BuilderSettings);
            builded.Add(obstacleBuilder);
            yield return obstacleBuilder.WaitPostDelay();
            obstacleBuilder.OnPostDelayOver();
        }

        foreach (ObstacleBuilder builder in levelData.Obstacles) {
            yield return builder.CanStop();
        }

        foreach (ObstacleBuilder builder in levelData.Obstacles) {
            builder.Cleanup();
        }


        builded = null;
        OnLevelOver?.Invoke(levelData);
    }
}