using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowUI : BaseUI
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.Translate(eventData.delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Manager.UI.SelectWindowUI(this);
    }

    public void Close()
    {
        Manager.UI.CloseWindowUI(this);
    }
}
