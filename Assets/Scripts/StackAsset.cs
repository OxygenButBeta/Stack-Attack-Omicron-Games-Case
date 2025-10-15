using UnityEngine;

[CreateAssetMenu(fileName = "StackAsset", menuName = "ScriptableObjects/StackAsset", order = 1)]
public class StackAsset : ScriptableObject {
    [field: SerializeField] public Color color { get; private set; }
    [field: SerializeField] public Vector2Int randomAmount { get; private set; }
}