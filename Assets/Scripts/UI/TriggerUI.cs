using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
public class TriggerUI : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Canvas PopUpUI;
    [SerializeField] LayerMask layermask;
    [SerializeField] GameObject Pointer;
    [SerializeField] bool Debug;

    private void Start()
    {
        if (!Debug)
            Pointer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layermask.Contain(other.gameObject.layer))
        {
            if (PopUpUI != null && player != null)
            {
                PopUI();
                Destroy(gameObject);
            }
        }
    }

    private void PopUI()
    {
        Canvas popUI = Instantiate(PopUpUI, transform.position, transform.rotation);
        popUI.transform.SetParent(this.player.transform);
        Manager.timeflow.timeStop = true;
    }
}
