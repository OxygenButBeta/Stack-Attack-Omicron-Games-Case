using UnityEngine;

public class VerticalFall : MonoBehaviour {
    public float SpeedModifier = 1f;

    void Update() {
        transform.position += Vector3.down * (SpeedModifier * (GameConfigurationAsset.Default.FallingSpeed * Time.deltaTime));
    }
}