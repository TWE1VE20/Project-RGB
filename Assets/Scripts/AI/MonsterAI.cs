using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour, IDamagable
{
    //[Header("target tring")]
    //[SerializeField] LayerMask redTeam;
    //[SerializeField] LayerMask blueTeam;

    //public List<BattleAI> blueAI = new List<BattleAI>();
    //public List<BattleAI> redAI = new List<BattleAI>(); 

    public float hitPoint;
    public void TakeDamage(int damage)
    {
        Debug.Log("damaged");
        hitPoint -= damage;
    }



    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    Debug.Log("plus");
    //    if (((1 << other.gameObject.layer) & blueTeam) != 0)
    //    {
    //        Debug.Log("blueteam plus");
    //        BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();

    //        blueAI.Add(battleAI);
    //    }

    //    if (((1 << other.gameObject.layer) & redTeam) != 0)
    //    {
    //        Debug.Log("redteam plus");
    //        BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
    //        redAI.Add(battleAI);
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    Debug.Log("minus");
    //    if (((1 << other.gameObject.layer) & blueTeam) != 0)
    //    {
    //        Debug.Log("blueteam minus");
    //        BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
    //        blueAI.Remove(battleAI);
    //    }
    //    if (((1 << other.gameObject.layer) & redTeam) != 0)
    //    {
    //        Debug.Log("redteam minus");
    //        BattleAI battleAI = other.gameObject.GetComponent<BattleAI>();
    //        redAI.Remove(battleAI);
    //    }
    //}
  
}
