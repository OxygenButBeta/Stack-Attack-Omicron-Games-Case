using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileRunner : Singleton<ProjectileRunner> {
    [ShowInInspector, ReadOnly] readonly List<ProjectileLifetimePair> projectileAssets = new();
    [SerializeField] Transform projectilePoolParent;
    AssetPoolMap<ProjectileAsset, ProjectileBase> projectilePoolMap;
    readonly Dictionary<ProjectileAsset, float> projectileCooldownOverrides = new();

    [Button, BoxGroup("Editor Invoke")]
    public void InjectProjectile(ProjectileAsset asset) {
        projectileAssets.Add(new ProjectileLifetimePair {
            projectileAsset = asset,
        });
    }

    public void OverrideProjectileCooldown(ProjectileAsset asset, float cooldown) {
        projectileCooldownOverrides.TryGetValue(asset, out var currentCooldownOverride);
        currentCooldownOverride += cooldown;
        projectileCooldownOverrides[asset] = currentCooldownOverride;
    }

    void Start() {
        InitializePools();
        foreach (BaseMapper<ProjectileAsset, ProjectileAssetAssetKey>.ValueKeyPair pair in ProjectileAssetMapper.Default
                     .Assets) {
            projectileCooldownOverrides.Add(pair.Asset, 0f);
        }
    }

    void InitializePools() {
        projectilePoolMap = PoolUtilities.BuildPoolsFromScriptableAssetsMap(
            ProjectileAssetMapper.Default.Assets.Select(x => x.Asset),
            createFunc: asset => {
                ProjectileBase proj = Instantiate(asset.ProjectilePrefab, transform.position, Quaternion.identity);
                proj.SetOnOutOfViewportCallback(projectile => projectilePoolMap.Release(asset, projectile));
                proj.SetOnHitCallback((projectile, receiver) => HandleProjectileHit(asset, projectile, receiver));
                return proj;
            },
            actionOnGet: (proj) => {
                proj.ResetProjectile();
                proj.transform.rotation = Quaternion.identity;
                proj.transform.SetParent(null);
                proj.gameObject.SetActive(true);
            },
            actionOnRelease: (proj) => {
                proj.ResetProjectile();
                proj.gameObject.SetActive(false);
                proj.transform.SetParent(projectilePoolParent);
            },
            actionOnDestroy: proj => {
                proj?.SetOnHitCallback(null);
                proj?.SetOnOutOfViewportCallback(null);
            },
            collectionCheck: false,
            defaultCapacity: 50
        );
    }

    void HandleProjectileHit(ProjectileAsset asset, ProjectileBase proj, IDamageReceiver arg2) {
        projectilePoolMap.Release(asset, proj);
    }

    void Update() {
        foreach (ProjectileLifetimePair pair in projectileAssets)
            pair.Tick(Time.deltaTime);

        if (!InputHelper.IsPointerDown())
            return;
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (ProjectileLifetimePair pair in projectileAssets) {
                projectileCooldownOverrides.TryGetValue(pair.projectileAsset, out var currentCooldownOverride);
                if (!pair.CanLaunch(currentCooldownOverride))
                    continue;

                pair.ResetTimer();
                ProjectileBase proj = projectilePoolMap.Get(pair.projectileAsset);
                proj.transform.position = transform.position;
                proj.Launch(Vector3.up);
            }
        }
    }

    public void Clear() {
        projectileAssets.Clear();
    }
}