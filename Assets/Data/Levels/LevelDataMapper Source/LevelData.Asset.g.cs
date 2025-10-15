
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the LevelData enum and provides
    // easy access to the corresponding LevelData asset.
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
    // Generated 1.0 10/13/2025 5:47:08 PM

    [System.Serializable]
    public struct LevelDataAsset {
        [UnityEngine.SerializeField] LevelDataAssetKey Key;
        LevelData cached;

        public LevelData Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator LevelData(LevelDataAsset asset) {
            return asset.Value; 
        }

        LevelData GetAssetDynamic() {
            return LevelDataMapper.GetMappedAsset(Key);
        }

        LevelData GetAssetAndCache() {
            if (!cached)
                cached = LevelDataMapper.GetMappedAsset(Key);

            return cached;
        }
        public LevelDataAsset(LevelDataAssetKey key) {
            cached = null;
            this.Key = key;
        }
    }
    