using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] GameObject grave;
    [SerializeField] Transform graveTransform;
    [SerializeField] LayerMask playerLayer;
    private string sceneNameText;


    void Start()
    {
        graveTransform = grave.transform;
        //AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlayBgm(AudioManager.BGM.InGame);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("respawnPoint change");
            Manager.game.respawnPoint = graveTransform.position;
        }

        //Manager.game.respwanScene = SceneManager.GetActiveScene().name;
    }



}
