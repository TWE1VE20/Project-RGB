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
    [SerializeField] SceneManager SceneManager;
    [SerializeField] DataManager dataManager;

    [Header("DebugUI")]
    [SerializeField] Canvas DebugCanvas;

    public static TimeFlowManager timeflow { get { return instance.timeflowManager; } }
    public static TimeControlStamina timestamina { get { return instance.timestaminaManage; } }
    public static GameManager game { get { return instance.gameManager; } }
    public static UIManager UI { get { return instance.UIManager; } }
    public static SceneManager Scene { get { return instance.SceneManager; } }
    public static DataManager data { get { return instance.dataManager; } }
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
