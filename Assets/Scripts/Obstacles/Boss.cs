using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageReceiver, ICellStack {
    public Transform Transform => transform;
    [SerializeField, Range(0, 1000)] float Health = 100;
    [SerializeReference] IDamageEffect[] damageEffects;
    [SerializeField] VFXPairAssetKey onDeathVFX;
    [SerializeField, Required] GameObject visualParent;
    public bool IsAlive { get; private set; } = true;

    public void OnDamage(float damage) {
        if (!IsAlive)
            return;

        foreach (IDamageEffect effect in damageEffects) {
            effect.StopImmediate();
            effect.ExecuteEffect(this);
        }

        Health -= damage;
        if (Health > 0)
            return;

        Health = 0;
        VFXPair vfx = ReferenceManager.VFXPoolMap.Get(onDeathVFX);
        vfx.SetPosition(transform.position);
        vfx.SetBeforeReleaseCallback(() => IsAlive = false);
        visualParent.SetActive(false);
    }

    IEnumerable<Cell> ICellStack.GetCells() {
        yield break;
    }
}