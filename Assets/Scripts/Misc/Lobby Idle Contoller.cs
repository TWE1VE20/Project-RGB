using QFX.SFX;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyIdleContoller : MonoBehaviour
{
    [SerializeField] SFX_ComponentsStartupController VFX;
    [SerializeField] float Delay;
    [SerializeField] GameObject VisibleIdle;

    private IEnumerator appeared;
    private bool appear;
    private void Start()
    {
        appear = false;
        VFX.Delay = this.Delay;
        VFX.enabled = true;
        appeared = WaitToAppear(2);
        StartCoroutine(appeared);
    }

    private void Update()
    {
        if (appear)
            VisibleIdle.SetActive(true);
            // InvisibleIdle.SetActive(false);
    }

    IEnumerator WaitToAppear(float time)
    {
        yield return new WaitForSeconds(time);
        appear = true;
    }
}
