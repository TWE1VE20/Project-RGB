using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2Scene : MonoBehaviour
{
    [SerializeField] PopUpUI DeadUIPrefab;
    public void DeadUIcall()
    {
        Manager.UI.ShowPopUpUI(DeadUIPrefab);
    }
}
