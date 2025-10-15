using DG.Tweening;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[System.Serializable]
public class TopCellDamageEffect : IDamageEffect {
    [SerializeField] float jumpStrength = 0.5f;
    [SerializeField] int vibrato = 10;
    [SerializeField] float elasticity = 1f;
    [SerializeField] float duration = 0.3f;
    [SerializeField] float rotationAmount = 15f;
    Sequence damageSequence;

    public bool IsPlaying => damageSequence.IsActive();


    public void ExecuteEffect(ICellStack stack) {
        damageSequence = DOTween.Sequence();
        Cell topCell = stack.GetLastCell();
        if (topCell == null)
            return;

        // Jump effect
        Transform t = topCell.transform;
        Vector3 originalPos = t.position;
        Quaternion originalRot = t.rotation;

        damageSequence = DOTween.Sequence();

        damageSequence.Join(
            t.DOPunchPosition(new Vector3(0, jumpStrength, 0), duration, vibrato, elasticity)
        );
        damageSequence.Join(
            t.DOPunchRotation(new Vector3(0, 0, Random.Range(-rotationAmount, rotationAmount)), duration, vibrato,
                elasticity)
        );

        damageSequence.OnComplete(() => {
            t.position = originalPos;
            t.rotation = originalRot;
        });
        damageSequence.onKill += (() => {
            t.position = originalPos;
            t.rotation = originalRot;
        });
    }

    public void StopImmediate() {
        damageSequence?.Kill();
    }
}