using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointer: MonoBehaviour
{
    [Header("Pointers")]
    public GameObject startPoint;
    public GameObject endPoint;

    private void Start()
    {
        startPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
        endPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
}
