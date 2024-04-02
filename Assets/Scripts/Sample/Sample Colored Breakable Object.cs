using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleColoredBreakableObject : HaveColor, IBreakable
{
    private bool breaked;

    private void Start()
    {
        breaked = false;
    }

    public void Break()
    {
        if (!breaked)
            StartCoroutine(StunWait());
            // 추가로 파편을 흩뿌리는 파티클이나 Debris Object를 소환하고 자신은 Destroy하도록 함
    }

    public void Break(Weapons.Colors color)
    {
        if (!breaked)
            StartCoroutine(StunWait());
            // 추가로 파편을 흩뿌리는 파티클이나 Debris Object를 소환하고 자신은 Destroy하도록 함
    }

    IEnumerator StunWait()
    {
        breaked = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        breaked = false;
    }
}
