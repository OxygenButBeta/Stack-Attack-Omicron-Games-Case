using DG.Tweening;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class BossBuilder : ObstacleBuilder {
    [SerializeField] float verticalDestination;
    [SerializeField] float moveDuration;

    Boss boss;

    public override void Build(in ObstacleBuilderSettings settings) {
        boss = Object.Instantiate(ReferenceManager.Instance.BossPrefab);
        boss.transform.position = GetSpawnPosition(in settings);
        boss.transform.DOMoveY(verticalDestination, moveDuration);
    }

    public override IEnumerator CanStop() {
        yield return new WaitUntil(() => !boss.IsAlive);
    }

    public override void Cleanup() {
        Object.Destroy(boss.gameObject);
    }
}