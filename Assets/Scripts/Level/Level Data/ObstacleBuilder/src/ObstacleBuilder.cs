using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class ObstacleBuilder {
    [ShowIf(nameof(HasDelay)), TabGroup("Delay")]
    public float StartDelay;

    [ShowIf(nameof(HasDelay)), TabGroup("Delay")]
    public float PostBuildDelay;

    [SerializeField, Range(0, 10), ShowIf(nameof(ShowHorizontalIndex))]
    int HorizontalIndex;
    protected virtual bool ShowHorizontalIndex => true;
    protected virtual bool HasDelay => true;
    protected Vector3 GetSpawnPosition(in ObstacleBuilderSettings settings) =>
        GetSpawnPosition(settings, HorizontalIndex);
    protected Vector3 GetSpawnPosition(in ObstacleBuilderSettings settings, int HIndex) {
        Vector2 pos = ViewportUtility.GetGridCenterWorldPos2D(ReferenceManager.Instance.CameraMain,
            new Vector2Int(HIndex, 0));
        return new Vector3(pos.x, settings.StartPosY, 0);
    }
    public abstract void Build(in ObstacleBuilderSettings settings);
    public virtual void OnPostDelayOver() { }
    public virtual void Cleanup() { }
    public virtual void Update() { }
    public virtual IEnumerator CanStop() => null;
    public virtual IEnumerator WaitPostDelay() {
        yield return new WaitForSeconds(PostBuildDelay);
    }
}