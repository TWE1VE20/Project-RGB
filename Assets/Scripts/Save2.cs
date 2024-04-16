using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save2 : MonoBehaviour
{
    [Header("save")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int savePosition;
    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Manager.game.savePos = savePosition;
        }
        Manager.data.SaveData();
        Debug.Log("savedata made");
        //Manager.game.respwanScene = SceneManager.GetActiveScene().name;
    }
}
