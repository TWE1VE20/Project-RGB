using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGamePopUi : MonoBehaviour
{
    private void OnEnable()
    {
        Manager.timeflow.timeStop = true;
    }

    public void Close()
    {
        Manager.timeflow.timeStop = false;
        Debug.Log("Close UI");
        Destroy(gameObject, 0.1f);
    }
}