using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] TextMeshPro textComponent;
    [SerializeField] SpriteRenderer spriteComponent;
    [ContextMenu("Run")]
    void FStart()
    {
        textComponent.sortingLayerID = spriteComponent.sortingLayerID;
        textComponent.sortingOrder = spriteComponent.sortingOrder + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
