using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] Transform cameraRoot;
    [SerializeField] Camera handcam;
    [SerializeField] float mouseSensitivity;

    [Header("Spec")]
    public float lerpSpeed = 1f;        // Lerp 속도
    public float InitFOV = 60;
    public float targetFOV = 60;        // 목표 FOV
    private float SlowmotionFOV;
    private bool Zoom;

    private Vector2 inputDir;
    private float yRotation;
    private float xRotation;

    private void Start()
    {
        cameraRoot.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = InitFOV;
        handcam.fieldOfView = InitFOV;
        SlowmotionFOV = InitFOV;
        Zoom = false;
    }

    private void FixedUpdate()
    {
        slowmotionZoom();
    }

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
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.Rotate(Vector3.up, inputDir.x * mouseSensitivity * Time.unscaledDeltaTime);
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void slowmotionZoom()
    {
        if (Zoom)
        {
            if (SlowmotionFOV != targetFOV)
            {
                SlowmotionFOV = Mathf.Lerp(SlowmotionFOV, targetFOV, lerpSpeed * Time.fixedDeltaTime);
                cameraRoot.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = SlowmotionFOV;
                handcam.fieldOfView = SlowmotionFOV;
            }
        }
        else
        {
            if (SlowmotionFOV != InitFOV)
            {
                SlowmotionFOV = Mathf.Lerp(SlowmotionFOV, InitFOV, lerpSpeed * Time.fixedDeltaTime);
                cameraRoot.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = SlowmotionFOV;
                handcam.fieldOfView = SlowmotionFOV;
            }
        }
    }

    private void OnLook(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    private void OnZoom(InputValue value)
    {
        if (value.isPressed)
            Zoom = true;
        else
            Zoom = false;
        
    }
}
