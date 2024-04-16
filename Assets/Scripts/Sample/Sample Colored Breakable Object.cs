using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SampleColoredBreakableObject : MonoBehaviour, IBreakable
{
    [Header("Script")]
    [SerializeField] HaveColor haveColor;

    [Header("Spec")]
    [SerializeField] HaveColor.ThisColor initColor;

    // public bool Died { get; private set; }

    private bool breaked;

    private void Awake()
    {
        if (haveColor == null)
        {
            haveColor = gameObject.AddComponent<HaveColor>();
            haveColor.SetColor(initColor);
        }
        haveColor.SetColor(initColor);
        gameObject.GetComponent<Renderer>().material.color = haveColor.MaterialColor();
    }
    private void Start()
    {
        breaked = false;
    }
    private void Update()
    {
        /* 만약 StateMachine을 이용중이라면 Transition에서
        if(owner.Died)
        {
            ChangeState(State.Die);
        }
        */
    }

    public void Break()
    {
        return;
    }

    public void Break(Weapons.Colors color)
    {
        haveColor.PeelColor(color);
        if (!breaked)
        {
            if (haveColor.curColor == HaveColor.ThisColor.BLACK)
            {
                // Died = true;
                StartCoroutine(BreakWait());
            }
            gameObject.GetComponent<Renderer>().material.color = haveColor.MaterialColor();
        }
    }

    IEnumerator BreakWait()
    {
        breaked = true;
        gameObject.GetComponent<Renderer>().material.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        haveColor.SetColor(initColor);
        gameObject.GetComponent<Renderer>().material.color = haveColor.MaterialColor();
        breaked = false;
    }
}
