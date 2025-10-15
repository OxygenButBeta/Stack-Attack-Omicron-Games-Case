using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ReactiveBackGround : MonoBehaviour {
    static readonly int Color1 = Shader.PropertyToID("Color");
    [SerializeField, Required] Material material;
    [SerializeField, Required] SpriteRenderer backgroundRenderer;


    [SerializeField] Color damageColor = Color.red;
    [SerializeField] float flashDuration = 0.2f;

    Material instanceMaterial;
    Color originalColor;

    void Awake() {
        backgroundRenderer.material = instanceMaterial = Instantiate(material);
        originalColor = instanceMaterial.color;
    }

    void Start() {
        Player.Instance.OnPlayerTookDamage += OnPlayerTookDamage;
    }

    void OnPlayerTookDamage() {
        instanceMaterial.DOKill();
        instanceMaterial.DOColor(damageColor, 0f);
        instanceMaterial.DOColor(originalColor, flashDuration).SetUpdate(true);
    }
}