using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField, Required] Renderer selfRenderer;

    [TabGroup("Movement Settings"), SerializeField]
    float acceleration = 10f;

    [TabGroup("Movement Settings"), SerializeField]
    float deceleration = 5f;

    [TabGroup("Movement Settings"), SerializeField]
    float minDragDistance = 0.1f;

    [TabGroup("Movement Settings"), SerializeField]
    Camera cam;

    [TabGroup("Rotation Settings"), SerializeField]
    float maxTiltAngle = 15f;

    [TabGroup("Rotation Settings"), SerializeField]
    float rotationSmooth = 10f;

    [TabGroup("Bounds"), SerializeField, Required]
    Renderer playerRenderer;

    Vector2 lastMousePos;
    float velocityX;
    float targetVelocityX;
    float halfWidth;

    void Awake() {
        halfWidth = selfRenderer.bounds.extents.x;
    }

    void Update() {
        HandleInput();
        ApplyMovement();
        ApplyRotation();
    }

    void HandleInput() {
        bool isDown = InputHelper.IsPointerDown();
        Vector2 pointerPos = InputHelper.GetPointerPosition();

        if (isDown) {
            if (lastMousePos == Vector2.zero)
                lastMousePos = pointerPos; // Dokunma/mouse başlangıcı

            Vector2 delta = pointerPos - lastMousePos;
            lastMousePos = pointerPos;

            if (Mathf.Abs(delta.x) < minDragDistance)
                return;

            Vector3 worldDelta = cam.ScreenToWorldPoint(
                new Vector3(pointerPos.x, pointerPos.y,
                    cam.WorldToScreenPoint(transform.position).z)
            ) - cam.ScreenToWorldPoint(
                new Vector3(pointerPos.x - delta.x, pointerPos.y,
                    cam.WorldToScreenPoint(transform.position).z)
            );

            targetVelocityX = worldDelta.x * GameConfigurationAsset.Default.Sensitivity * 60f;
        }
        else {
            lastMousePos = Vector2.zero;
            targetVelocityX = 0;
        }
    }

    void ApplyMovement() {
        var lerpSpeed = targetVelocityX == 0 ? deceleration : acceleration;
        velocityX = Mathf.Lerp(velocityX, targetVelocityX, Time.deltaTime * lerpSpeed);

        Vector3 newPos = transform.position + new Vector3(velocityX * Time.deltaTime, 0f, 0f);

        Vector3 minScreen = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.WorldToScreenPoint(transform.position).z));
        Vector3 maxScreen = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.WorldToScreenPoint(transform.position).z));

        newPos.x = Mathf.Clamp(newPos.x, minScreen.x + halfWidth, maxScreen.x - halfWidth);

        transform.position = newPos;
    }

    void ApplyRotation() {
        if (Mathf.Abs(velocityX) < 0.01f) {
            Quaternion targetRot2 = Quaternion.identity;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot2, Time.deltaTime * rotationSmooth);
            return;
        }

        var normalizedTilt = Mathf.Clamp(velocityX * 0.1f, -1f, 1f);
        var tilt = -normalizedTilt * maxTiltAngle;

        Quaternion targetRot = Quaternion.Euler(0f, 0f, tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSmooth);
    }
}