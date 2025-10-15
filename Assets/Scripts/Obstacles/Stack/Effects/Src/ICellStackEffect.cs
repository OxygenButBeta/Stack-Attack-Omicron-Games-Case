
public interface ICellStackEffect {
    bool IsPlaying { get; }
    public void ExecuteEffect(ICellStack stack);
    public void StopImmediate();
}
