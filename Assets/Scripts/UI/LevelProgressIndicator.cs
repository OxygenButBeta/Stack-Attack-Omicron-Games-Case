using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressIndicator : MonoBehaviour {
    [SerializeField] Slider progressSlider;

    bool TimerOn;

    void Start() {
        LevelRunner.Instance.OnLevelStarted += UpdateProgress;
    }

    void UpdateProgress(LevelData levelData) {
        var totalLevelTime = levelData.Obstacles.Sum(builder => builder.StartDelay + builder.PostBuildDelay);
        progressSlider.maxValue = totalLevelTime;
        progressSlider.value = 0;
        TimerOn = true;
    }

    void Update() {
        if (TimerOn)
            progressSlider.value += Time.deltaTime;
    }
}