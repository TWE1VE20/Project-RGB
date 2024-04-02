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
        // 만약 stuned가 true가 된다면 StateMachine에서 Stun State에 진입
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
