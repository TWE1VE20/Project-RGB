using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleStunObject : MonoBehaviour, IStunable
{
    private bool stuned;

    private void Start()
    {
        stuned = false;
    }

    private void Update()
    {
        // ���� stuned�� true�� �ȴٸ� StateMachine���� Stun State�� ����
    }

    public void Stun()
    {
        if(!stuned)
            StartCoroutine(StunWait());
    }

    IEnumerator StunWait() 
    {
        stuned = true;
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        stuned = false;
    }
}
