using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour {
    [field: SerializeField] public SpriteRenderer spriteComponent { get; private set; }

    int lastSortingLayerID;
    public void SetOrderInLayer(int i) => spriteComponent.sortingOrder = i;
    public bool IsRequiredToChangeSortingLayer(int i) => lastSortingLayerID != i;
    public void SetOrderLayer(int i) {
        lastSortingLayerID = i;
        spriteComponent.sortingLayerName = "Cell" + i;
    }

}