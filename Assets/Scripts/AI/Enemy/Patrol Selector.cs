using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolSelector : MonoBehaviour
{
    [Header("Pointers")]
    public GameObject firstPoint;
    public GameObject secondPoint;
    public EnemyAI enemy;
    
    private void Start()
    {
        enemy.patrolPosition1 = firstPoint.transform.position;
        enemy.patrolPosition2 = secondPoint.transform.position;
        
        if (enemy.patrolPosition1 != null && enemy.patrolPosition2 != null)
        {
            firstPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
            secondPoint.GetComponentInChildren<Transform>().gameObject.SetActive(false);
        }
        
    }
}
