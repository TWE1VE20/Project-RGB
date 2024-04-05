using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : MonoBehaviour 
{
    [SerializeField] GameObject grave;
    [SerializeField] LayerMask Player;
    private string sceneNameText;
    public void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & Player) != 0)
        { 
            Manager.game.respawnPoint = grave.transform.position; 
        }
        
        //Manager.game.respwanScene = SceneManager.GetActiveScene().name;
    }
    


}
