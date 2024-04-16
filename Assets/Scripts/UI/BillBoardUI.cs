using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardUi : BaseUI
{
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
