using TMPro;
using System;
using System.Linq;
using UnityEngine;
using System.Buffers;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class VerticalCellStack : MonoBehaviour, IDamageReceiver, ISelfPoolReleaseListener, ISelfPoolGetListener,
    ICellStack, IPlayerDamager {
    const float VerticalOffsetPerCell = .1f;

    public event Action<VerticalCellStack> OnKillEvent;

    [TabGroup("References"), Required, SerializeField]
    Sprite defaultSpriteAsset;

    [TabGroup("References"), Required, SerializeField]
    TextMeshPro healthIndicatorText;

    [TabGroup("References"), Required, SerializeField]
    CapsuleCollider2D selfCollider;

    [TabGroup("Configuration"), SerializeField, Range(0, 20)]
    int maxVisualCellAmount = 10;

    [TabGroup("Configuration"), SerializeField]
    SFXAsset destroySFX;


    [TabGroup("Configuration"), SerializeField]
    VFXPairAssetKey vfxOnDestroy;

    [TabGroup("Configuration"), SerializeReference, SerializeField]
    ICellStackEffect[] stackEffects;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    int visualCellAmount;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    StackAsset stackAsset;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    int initialStackAmount;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    int currentStackAmount;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    float damageRemainder;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    float stackPerCell;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    Cell[] cellReferences;

    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    public bool ViewportCheck = true;


    [TabGroup("Read-Only Debug Data"), ReadOnly, ShowInInspector]
    bool InPool, beenInViewport;

    Action<VerticalCellStack> OnOutOfBoundsCallback;
    Transform healthIndicatorTransform;

    //Implementation of ICellStack
    Transform ICellStack.Transform => transform;

    IEnumerable<Cell> ICellStack.GetCells() {
        if (cellReferences is null)
            yield break;
        for (var i = 0; i < visualCellAmount; i++) {
            yield return cellReferences[i];
        }
    }

    void Awake() {
        healthIndicatorTransform = healthIndicatorText.transform.parent;
    }

    [BoxGroup("Dynamic Invokes"), Button]
    public void SetStackAsset(StackAsset targetStackAsset, int stackSize = -1) {
        DisposeCurrentCells();
        damageRemainder = 0f;
        stackAsset = targetStackAsset;
        stackSize = stackSize == -1
            ? Random.Range(targetStackAsset.randomAmount.x, targetStackAsset.randomAmount.y)
            : stackSize;
        initialStackAmount = currentStackAmount = stackSize;
        visualCellAmount = Mathf.Clamp(stackSize, 0, maxVisualCellAmount);
        stackPerCell = (float)initialStackAmount / visualCellAmount;
        if (stackPerCell <= 0f)
            stackPerCell = 1f;

        OnOutOfBoundsCallback = RelaseToPool;


        RentCellVisuals();
        ArrangeVisualCellPositions();
        SetHealthIndicatorPosition();
    }

    public void SetOnOutOfBoundsCallback(Action<VerticalCellStack> callback) => OnOutOfBoundsCallback = callback;

    void RelaseToPool(VerticalCellStack stack) => StaticPool<VerticalCellStack>.Release(stack);

    void SetHealthIndicatorPosition() {
        healthIndicatorText.text = currentStackAmount.ToString();
        Cell lastCell = cellReferences[visualCellAmount - 1];
        healthIndicatorText.sortingLayerID = lastCell.spriteComponent.sortingLayerID;
        healthIndicatorText.sortingOrder = lastCell.spriteComponent.sortingOrder + 1;
        healthIndicatorTransform.localPosition = lastCell.transform.localPosition;
    }

    void ArrangeVisualCellPositions() {
        for (var i = 0; i < visualCellAmount; i++) {
            Cell currentCell = cellReferences[i];
            currentCell.transform.SetParent(transform);

            Vector3 localPos = i * VerticalOffsetPerCell * Vector3.up;
            currentCell.transform.localPosition = localPos;

            currentCell.spriteComponent.sprite = defaultSpriteAsset;
            currentCell.spriteComponent.color = stackAsset.color;
            currentCell.SetOrderInLayer(i);
        }

        ArrangeCollider();
    }

    static void CalculateColliderOffsetAndSize(int n, out float offsetY, out float sizeY) {
        const float n1 = 5f, n2 = 9f;
        const float offset1 = 0.1570587f, offset2 = 0.4112955f;
        const float size1 = 1.264117f, size2 = 1.772591f;

        var t = (n - n1) / (n2 - n1);
        offsetY = Mathf.Lerp(offset1, offset2, t);
        sizeY = Mathf.Lerp(size1, size2, t);
    }

    void ArrangeCollider() {
        CalculateColliderOffsetAndSize(visualCellAmount, out var offsetY, out var sizeY);
        selfCollider.offset = new Vector2(selfCollider.offset.x, offsetY);
        selfCollider.size = new Vector2(selfCollider.size.x, sizeY);
    }

    void DisposeCurrentCells() {
        if (cellReferences is null)
            return;

        foreach (Cell cellReference in cellReferences) {
            if (cellReference)
                StaticPool<Cell>.Release(cellReference);
        }

        ArrayPool<Cell>.Shared.Return(cellReferences, true);
        cellReferences = null;
    }

    void OnEnable() {
        StartCoroutine(CheckAndSetSortingLayerLoop());
    }

    void RentCellVisuals() {
        cellReferences = ArrayPool<Cell>.Shared.Rent(visualCellAmount);
        for (var i = 0; i < visualCellAmount; i++)
            cellReferences[i] = StaticPool<Cell>.Get();
    }

    IEnumerator CheckAndSetSortingLayerLoop() {
        while (true) {
            CheckAndSetSortingLayer();
            yield return new WaitForSeconds(0.2f);
        }
        // ReSharper disable once IteratorNeverReturns
    }


    void LateUpdate() {
        if (!ViewportCheck || InPool)
            return;

        if (ViewportUtility.IsVerticalOutOfViewport(selfCollider, ReferenceManager.Instance.CameraMain)) {
            if (beenInViewport)
                OnOutOfBoundsCallback(this);
        }
        else
            beenInViewport = true;
    }

    void CheckAndSetSortingLayer() {
        if (cellReferences is null || InPool)
            return;

        for (var i = 0; i < visualCellAmount; i++) {
            Cell currentCell = cellReferences[i];
            if (!currentCell)
                continue;
            var targetLayer =
                ViewportUtility.GetVerticalLayerIndex(transform, ReferenceManager.Instance.CameraMain, 15);
            if (!currentCell.IsRequiredToChangeSortingLayer(targetLayer))
                continue;
            currentCell.SetOrderLayer(targetLayer);
            if (i == visualCellAmount - 1)
                SetHealthIndicatorPosition();
        }
    }

    [BoxGroup("Dynamic Invokes"), Button]
    public void OnDamage(float damage) {
        if (visualCellAmount <= 0)
            return;

        damageRemainder += damage;
        currentStackAmount -= Mathf.FloorToInt(damage);
        healthIndicatorText.text = currentStackAmount.ToString();

        var cellsToRemove = Mathf.FloorToInt(damageRemainder / stackPerCell);

        if (cellsToRemove <= 0) {
            PlayDamageEffects();
            return;
        }

        cellsToRemove = Mathf.Min(cellsToRemove, visualCellAmount);

        if (cellReferences is not null) {
            for (var i = 0; i < cellsToRemove; i++) {
                var idx = visualCellAmount - 1 - i;
                Cell cell = cellReferences[idx];
                StaticPool<Cell>.Release(cell);
                cellReferences[idx] = null;
            }
        }

        visualCellAmount -= cellsToRemove;
        damageRemainder -= cellsToRemove * stackPerCell;

        if (damageRemainder < 0f)
            damageRemainder = 0f;

        if (currentStackAmount <= 0) { 
            ReferenceManager.Instance.AudioSource.PlayOneShot(destroySFX);
            ReferenceManager.VFXPoolMap.Get(vfxOnDestroy).SetPosition(transform.position);
            OnKillEvent?.Invoke(this);
            RelaseToPool(this);
            return;
        }

        ArrangeVisualCellPositions();
        SetHealthIndicatorPosition();
        PlayDamageEffects();
    }


    void PlayDamageEffects() {
        foreach (IDamageEffect effect in stackEffects.OfType<IDamageEffect>()) {
            if (effect.IsPlaying)
                effect.StopImmediate();

            effect.ExecuteEffect(this);
        }
    }


    public void OnSelfRelease() {
        foreach (ICellStackEffect effect in stackEffects)
            effect.StopImmediate();
        DisposeCurrentCells();
        InPool = true;
        //visualCellAmount = 0;
        damageRemainder = 0f;
    }

    public void OnSelfGet() {
        ViewportCheck = true;
        beenInViewport = false;
        InPool = false;
    }

    public int OnCollideWithPlayer() {
        ReferenceManager.VFXPoolMap.Get(vfxOnDestroy).SetPosition(transform.position);
        StaticPool<VerticalCellStack>.Release(this);
        return 1;
    }
}