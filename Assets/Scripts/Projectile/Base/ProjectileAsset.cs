using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileAsset", menuName = "ScriptableObjects/ProjectileAsset")]
public class ProjectileAsset : ScriptableObject {
    [field: SerializeField] public string ProjectileName { get; private set; }
    [field: SerializeField] public ProjectileBase ProjectilePrefab { get; private set; }
    [field: SerializeField] public float ProjectileCooldown { get; private set; }
}