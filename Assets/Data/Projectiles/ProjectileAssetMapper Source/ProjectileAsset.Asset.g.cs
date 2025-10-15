
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the ProjectileAsset enum and provides
    // easy access to the corresponding ProjectileAsset asset.
    //
    // Features:
    // - Implicit conversion to AudioClip for convenience.
    // - Caches the loaded asset for runtime efficiency.
    // - Uses dynamic loading in the Editor for live updates.
    //
    // WARNING:
    // - This code is auto-generated; do NOT modify manually.
    //
    // ==============================
    // Generated 1.0 10/10/2025 4:44:20 PM

    [System.Serializable]
    public struct ProjectileAssetBind {
        [UnityEngine.SerializeField] ProjectileAssetAssetKey Key;
        ProjectileAsset cached;

        public ProjectileAsset Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator ProjectileAsset(ProjectileAssetBind asset) {
            return asset.Value; 
        }

        ProjectileAsset GetAssetDynamic() {
            return ProjectileAssetMapper.GetMappedAsset(Key);
        }

        ProjectileAsset GetAssetAndCache() {
            if (!cached)
                cached = ProjectileAssetMapper.GetMappedAsset(Key);

            return cached;
        }
        public ProjectileAssetBind(ProjectileAssetAssetKey key) {
            cached = null;
            this.Key = key;
        }
    }
    