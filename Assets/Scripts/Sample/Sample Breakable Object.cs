using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBreakableObject : MonoBehaviour, IBreakable
{
    private bool breaked;

    private void Start()
    {
        breaked = false;
    }

    public void Break()
    {
        if (!breaked)
            StartCoroutine(BreakWait());
            // �߰��� ������ ��Ѹ��� ��ƼŬ�̳� Debris Object�� ��ȯ�ϰ� �ڽ��� Destroy�ϵ��� ��
    }

    public void Break(Weapons.Colors color)
    {
        if (!breaked)
            StartCoroutine(BreakWait());
            // �߰��� ������ ��Ѹ��� ��ƼŬ�̳� Debris Object�� ��ȯ�ϰ� �ڽ��� Destroy�ϵ��� ��
    }

    IEnumerator BreakWait()
    {
        breaked = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        breaked = false;
    }
}
