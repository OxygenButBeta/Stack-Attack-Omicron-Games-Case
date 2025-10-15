using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StackRotator : ObstacleBuilder {
    [SerializeField] int totalAmount = 5;
    [SerializeField] float radius = 1;
    [SerializeField] StackAssetBind[] stackAsset = Array.Empty<StackAssetBind>();
    [SerializeField] float rotationSpeedDeg = 30f;
    [SerializeField] float fallingSpeedFactor = 1f;

    readonly List<MovingStack> movingStacks = new();
    Vector2 originPos;

    class MovingStack {
        public VerticalCellStack stack;
        public float angleRad;
        public float distance;
        public float angularSpeedRad;
        public Quaternion initialRot;
    }

    public override void Build(in ObstacleBuilderSettings settings) {
        originPos = GetSpawnPosition(in settings);
        originPos.y = settings.StartPosY + radius / 2f;

        var baseAngleStep = Mathf.PI * 2f / Mathf.Max(1, totalAmount);


        for (var i = 0; i < totalAmount; i++) {
            var startAngle = i * baseAngleStep;
            var speedRad = rotationSpeedDeg * Mathf.Deg2Rad;

            VerticalCellStack v = GetStack();
            v.OnKillEvent += Untrack;
            v.SetOnOutOfBoundsCallback((stack) => {
                Untrack(stack);
                StaticPool<VerticalCellStack>.Release(stack);
            });

            Vector2 pos = originPos + new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle)) * radius;
            v.transform.position = new Vector3(pos.x, pos.y, 0);
            Quaternion initialRot = v.transform.rotation;

            movingStacks.Add(new MovingStack {
                stack = v,
                angleRad = startAngle,
                distance = radius,
                angularSpeedRad = speedRad,
                initialRot = initialRot
            });
        }
    }

    void Untrack(VerticalCellStack obj) {
        MovingStack moveData = movingStacks.Find(ms => ms.stack == obj);
        if (moveData != null)
            movingStacks.Remove(moveData);

        obj.OnKillEvent -= Untrack;
    }

    public override void Update() {
        if (movingStacks.Count == 0)
            return;


        var dt = Time.deltaTime;
        var falling = GameConfigurationAsset.Default.FallingSpeed * fallingSpeedFactor;

        originPos.y -= falling * dt;

        foreach (MovingStack ms in movingStacks) {
            ms.angleRad += ms.angularSpeedRad * dt;
            Vector2 offset = new Vector2(Mathf.Cos(ms.angleRad), Mathf.Sin(ms.angleRad)) * ms.distance;
            Vector2 newPos = originPos + offset;
            ms.stack.transform.position = new Vector3(newPos.x, newPos.y, 0);
            ms.stack.transform.rotation = ms.initialRot;
        }
    }

    public override void Cleanup() {
        if (movingStacks == null || movingStacks.Count == 0)
            return;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (MovingStack ms in movingStacks) {
            if (!ms.stack) continue;
            StaticPool<VerticalCellStack>.Release(ms.stack);
        }

        movingStacks.Clear();
    }

    VerticalCellStack GetStack() {
        VerticalCellStack vStack = StaticPool<VerticalCellStack>.Get();
        //  vStack.ViewportCheck = false;
        vStack.SetStackAsset(stackAsset[UnityEngine.Random.Range(0, stackAsset.Length)]);
        return vStack;
    }
}