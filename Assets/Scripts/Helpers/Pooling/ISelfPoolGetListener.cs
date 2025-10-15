public interface ISelfPoolGetListener {
    /// <summary>
    ///  Will be called when the object is taken from the pool.
    /// </summary>
    void OnSelfGet();
}