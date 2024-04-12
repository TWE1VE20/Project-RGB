using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGamePopUi : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;       
    }

    public void Close()
    {
        Debug.Log("Close UI");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
