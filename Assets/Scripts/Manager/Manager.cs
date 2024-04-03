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
    [SerializeField] GameManager gameManager;

    [Header("DebugUI")]
    [SerializeField] Canvas DebugCanvas;

    public static TimeFlowManager timeflow { get { return instance.timeflowManager; } }
    public static GameManager game { get { return instance.gameManager; } } 
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
    // 叼滚弊 UI 包府侩 备开
    private void OnTimeDebug(InputValue value)
    {
        DebugCanvas.gameObject.SetActive(!DebugCanvas.gameObject.activeSelf);
        Debug.Log($"DebugCanvas {DebugCanvas.gameObject.activeSelf}");
    }
    #endregion
}
