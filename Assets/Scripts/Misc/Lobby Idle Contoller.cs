using QFX.SFX;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyIdleContoller : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] invisibleObjects;
    [SerializeField] SFX_ComponentsStartupController VFX;
    [SerializeField] float Delay;
    private IEnumerator appeared;
    private bool appear;
    private void Start()
    {
        appear = false;
        foreach (SkinnedMeshRenderer obj in invisibleObjects)
        {
            Color objMaterial = obj.materials[0].color;
            obj.materials[0].color = new Color(objMaterial.r, objMaterial.g, objMaterial.b, 0);
        }
        VFX.Delay = this.Delay;
        VFX.enabled = true;
        appeared = WaitToAppear(Delay);
        StartCoroutine(appeared);
    }

    private void Update()
    {
        if(appear)
            foreach (SkinnedMeshRenderer obj in invisibleObjects)
            {
                Color objMaterial = obj.materials[0].color;
                obj.materials[0].color = new Color(objMaterial.r, objMaterial.g, objMaterial.b, 255);
            }
    }

    IEnumerator WaitToAppear(float time)
    {
        yield return new WaitForSeconds(time);
        appear = true;
    }
}
