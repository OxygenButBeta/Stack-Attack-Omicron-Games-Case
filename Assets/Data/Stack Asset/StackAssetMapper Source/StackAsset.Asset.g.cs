
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the StackAsset enum and provides
    // easy access to the corresponding StackAsset asset.
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
    // Generated 1.0 10/10/2025 7:24:35 PM

    [System.Serializable]
    public struct StackAssetBind {
        [UnityEngine.SerializeField] StackAssetAssetKey Key;
        StackAsset cached;

        public StackAsset Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator StackAsset(StackAssetBind asset) {
            return asset.Value; 
        }

        StackAsset GetAssetDynamic() {
            return StackAssetMapper.GetMappedAsset(Key);
        }

        StackAsset GetAssetAndCache() {
            if (!cached)
                cached = StackAssetMapper.GetMappedAsset(Key);

            return cached;
        }
        public StackAssetBind(StackAssetAssetKey key) {
            cached = null;
            this.Key = key;
        }
    }
    