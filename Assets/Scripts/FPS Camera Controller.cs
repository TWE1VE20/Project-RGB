using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;

    private Vector2 inputDir;
    private float yRotation;
    private float xRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        // unscaledDeltaTime으로 TimeScale 즉 슬로우모션의 영향에서 벗어난다.
        xRotation -= inputDir.y * mouseSensitivity * Time.unscaledDeltaTime;
        // 범위내에서만 있을수 있도록 조정하는 함수
        xRotation = Mathf.Clamp(xRotation, -80f, 40f);

        transform.Rotate(Vector3.up, inputDir.x * mouseSensitivity * Time.unscaledDeltaTime);
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void OnLook(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }
}
