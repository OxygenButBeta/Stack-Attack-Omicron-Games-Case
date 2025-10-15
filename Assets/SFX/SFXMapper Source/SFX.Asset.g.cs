
    // O2 UNITY AUTO ASSET MAPPER
    // ==============================
    // Auto Generated Asset Wrapper Struct
    //
    // This struct wraps the SFX enum and provides
    // easy access to the corresponding UnityEngine.AudioClip asset.
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
    // Generated 1.0 10/13/2025 12:55:08 AM

    [System.Serializable]
    public struct SFXAsset {
        [UnityEngine.SerializeField] SFXAssetKey Key;
        UnityEngine.AudioClip cached;

        public UnityEngine.AudioClip Value {
            get {
        #if UNITY_EDITOR
                return GetAssetDynamic();
        #else
                return GetAssetAndCache();
        #endif
            }
        }

        public static implicit operator UnityEngine.AudioClip(SFXAsset asset) {
            return asset.Value; 
        }

        UnityEngine.AudioClip GetAssetDynamic() {
            return SFXMapper.GetMappedAsset(Key);
        }

        UnityEngine.AudioClip GetAssetAndCache() {
            if (!cached)
                cached = SFXMapper.GetMappedAsset(Key);

            return cached;
        }
        public SFXAsset(SFXAssetKey key) {
            cached = null;
            this.Key = key;
        }
    }
    