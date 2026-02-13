using UnityEngine;
using System.Runtime.CompilerServices;

public static class ViewportUtility {
    public static Vector3 GetGridCenterWorldPos2D(
        Camera camera, Vector2Int targetGridIndex,
        float depth = 0f) {
        Vector3 pos3d = GetGridCenterWorldPos(camera, targetGridIndex, depth);
        return new Vector3(pos3d.x, pos3d.y, 0f);
    }
    public static Vector3 GetScreenTopLeftWorldPosition(Camera cam)
    {
        Vector3 screenPos = new(0, Screen.height, cam.nearClipPlane);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        return worldPos;
    }
    static Vector3 GetGridCenterWorldPos(
        Camera camera, Vector2Int targetGridIndex,
        float depth = 0f) {
        var gridX = targetGridIndex.x;
        var gridY = targetGridIndex.y;
        var gridCountX = GameConfigurationAsset.Default.ScreenPosToGridResolution.x;
        var gridCountY = GameConfigurationAsset.Default.ScreenPosToGridResolution.y;

        if (!camera) {
            Debug.LogError("Camera is null!");
            return Vector3.zero;
        }

        gridX = Mathf.Clamp(gridX, 0, gridCountX - 1);
        gridY = Mathf.Clamp(gridY, 0, gridCountY - 1);

        var cellWidth = 1f / gridCountX;
        var cellHeight = 1f / gridCountY;

        var viewportX = (gridX + 0.5f) * cellWidth;
        var viewportY = (gridY + 0.5f) * cellHeight;

        Vector3 viewportPos = new(viewportX, viewportY, depth);
        return camera.ViewportToWorldPoint(viewportPos);
    }

    public static int GetVerticalLayerIndex(Transform t, Camera cam, int divisions = 10) {
        Vector3 viewportPos = cam.WorldToViewportPoint(t.position);
        var normalizedY = Mathf.Clamp01(viewportPos.y);
        var index = (1 - normalizedY) * divisions;
        return Mathf.RoundToInt(index);
    }

    public static bool IsOutOfViewport(Collider2D collider, Camera camera) {
        if (!collider || !camera)
            return true;

        Bounds b = collider.bounds;

        Vector3 min = b.min;
        Vector3 max = b.max;

        return !IsPointVisible(min.x, min.y) &&
               !IsPointVisible(min.x, max.y) &&
               !IsPointVisible(max.x, min.y) &&
               !IsPointVisible(max.x, max.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsPointVisible(float x, float y) {
            Vector3 vp = camera.WorldToViewportPoint(new Vector3(x, y, b.center.z));
            return vp is { z: > 0, x: >= 0f and <= 1f, y: >= 0f and <= 1f };
        }
    }

    public static bool IsOutOfViewport(Vector2 center, float radius, Camera camera) {
        if (!camera)
            return true;

        return !IsPointVisible(center.x - radius, center.y - radius) &&
               !IsPointVisible(center.x - radius, center.y + radius) &&
               !IsPointVisible(center.x + radius, center.y - radius) &&
               !IsPointVisible(center.x + radius, center.y + radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsPointVisible(float x, float y) {
            Vector3 vp = camera.WorldToViewportPoint(new Vector3(x, y, 0f));
            return vp is { z: > 0, x: >= 0f and <= 1f, y: >= 0f and <= 1f };
        }
    }

    public static bool IsVerticalOutOfViewport(CapsuleCollider2D collider, Camera camera) {
        if (!collider || !camera)
            return false;

        var top = collider.transform.position.y + collider.offset.y + collider.size.y * 0.5f;
        var bottom = collider.transform.position.y + collider.offset.y - collider.size.y * 0.5f;

        var viewportTop = camera.WorldToViewportPoint(new Vector3(0f, top, 0f)).y;
        var viewportBottom = camera.WorldToViewportPoint(new Vector3(0f, bottom, 0f)).y;

        return viewportTop < 0f || viewportBottom > 1f;
    }

    public static Vector3 GetTopCenterPosition(Camera camera) {
        if (!camera)
            return Vector3.zero;

        var z = camera.orthographic ? 0f : camera.nearClipPlane;
        Vector3 viewportPoint = new(0.5f, 1f, z);
        return camera.ViewportToWorldPoint(viewportPoint);
    }
}