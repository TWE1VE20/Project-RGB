using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    private static Manager instance;
    public static Manager Inst { get { return instance; } }

    [Header("Managers")]
    [SerializeField] TimeFlowManager timeflowManager;
    [SerializeField] TimeControlStamina timestaminaManage;
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager UIManager;

    [Header("DebugUI")]
    [SerializeField] Canvas DebugCanvas;

    public static TimeFlowManager timeflow { get { return instance.timeflowManager; } }
    public static TimeControlStamina timestamina { get { return instance.timestaminaManage; } }
    public static GameManager game { get { return instance.gameManager; } }
    public static UIManager UI { get { return instance.UIManager; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DebugCanvas.gameObject.SetActive(false);
    }

    #region DebugUI
    // 디버그 UI 관리용 구역
    private void OnTimeDebug(InputValue value)
    {
        DebugCanvas.gameObject.SetActive(!DebugCanvas.gameObject.activeSelf);
        Debug.Log($"DebugCanvas {DebugCanvas.gameObject.activeSelf}");
    }
    #endregion
}
