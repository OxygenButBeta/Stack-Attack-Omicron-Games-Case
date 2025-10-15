public interface ISelfPoolReleaseListener {
    /// <summary>
    /// Will be called when the object is released back to the pool.
    /// </summary>
    void OnSelfRelease();
}