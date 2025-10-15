
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the VFXPair enum and provides
    // easy access to the corresponding VFXPair asset.
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
    // Generated 1.0 10/13/2025 1:34:02 AM

    [System.Serializable]
    public struct VFXPairAsset {
        [UnityEngine.SerializeField] VFXPairAssetKey Key;
        VFXPair cached;

        public VFXPair Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator VFXPair(VFXPairAsset asset) {
            return asset.Value; 
        }

        VFXPair GetAssetDynamic() {
            return VFXPairMapper.GetMappedAsset(Key);
        }

        VFXPair GetAssetAndCache() {
            if (!cached)
                cached = VFXPairMapper.GetMappedAsset(Key);

            return cached;
        }
        public VFXPairAsset(VFXPairAssetKey key) {
            cached = null;
            this.Key = key;
        }
    }
    