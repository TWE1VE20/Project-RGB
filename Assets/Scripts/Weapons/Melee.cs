using UnityEngine;

public class Melee : Weapons
{
    [Header("Componets")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] meleeAudioClip;
    [SerializeField] ReflectSystem reflectSystem;

    [Header("meleeStatus")]
    [SerializeField] float timeforAttack;

    private void Awake()
    {
        base.Starting();
        this.attackType = Weapons.AttackType.MELEE;
        SetAttackTime(timeforAttack);
        if (reflectSystem == null)
        {
            reflectSystem = gameObject.AddComponent<ReflectSystem>();
        }
        reflectSystem.animator = this.animator;
        reflectSystem.audioSource = this.audioSource;
    }

    public void OnEnable()
    {
        audioSource.clip = meleeAudioClip[0];
        if (audioSource.clip != null)
            audioSource.Play();
    }

    public override bool Attack()
    {
        // �������� ���
        /*
        audioSource.clip = meleeAudioClip[1];
        if (audioSource.clip != null)
            audioSource.Play();
        */
        return true;
    }
}
