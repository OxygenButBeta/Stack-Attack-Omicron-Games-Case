using UnityEngine;

public class HealthIndicator : MonoBehaviour {
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform heartContainer;

    void Start() {
        Player.Instance.OnHealthChanged += UpdateHealth;
    }

    void UpdateHealth(int obj) {
        for (var i = 0; i < heartContainer.childCount; i++)
            Destroy(heartContainer.GetChild(i).gameObject);

        for (var i = 0; i < obj; i++)
            Instantiate(heartPrefab, heartContainer);
    }

    void OnDestroy() {
        if (Player.Instance)
            Player.Instance.OnHealthChanged -= UpdateHealth;
    }
}