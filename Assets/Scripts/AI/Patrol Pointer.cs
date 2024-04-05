using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointer: MonoBehaviour
{
    [Header("Pointers")]
    public GameObject startPoint;
    public GameObject endPoint;
    public HumanMonster human;
    private void Start()
    {
        human.patrolPosition1 = startPoint.transform.position;
        human.patrolPosition2 = endPoint.transform.position;
        if (human.patrolPosition1 != null && human.patrolPosition2 != null)
        {
            startPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
            endPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
        }
        
    }
}
