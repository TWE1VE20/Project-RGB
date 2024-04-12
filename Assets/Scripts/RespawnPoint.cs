using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour 
{
    [SerializeField] GameObject grave;
    [SerializeField] LayerMask playerLayer;
    private string sceneNameText;
    public void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("respawnPoint change");
            Manager.game.respawnPoint = grave.transform.position;
        }
        
        //Manager.game.respwanScene = SceneManager.GetActiveScene().name;
    }
    


}
