using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoSystem: MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    [SerializeField] FireEffect fireEffect;

    public float reloadTime;
    public int AmmoLeft;    // ³²Àº ÃÑ¾Ë °¹¼ö
    public int maxRounds;   // ÅºÃ¢ ÃÖ´ë Å©±â
    public int rounds { get; private set; }     // ÇöÁ¦ ÃÑ¿¡ µé¾îÀÖ´Â ÃÑ¾Ë °¹¼ö

    private void Start()
    {
        this.rounds = maxRounds;
    }

    public void Reload()
    {
        if (AmmoLeft >= maxRounds)
        {
            AmmoLeft -= maxRounds - rounds;
            rounds = maxRounds;
            Debug.Log($"Reload {rounds} Rounds");
            return;
        }
        else if (AmmoLeft > 0)
        {
            if (rounds == 0)
            {
                rounds = AmmoLeft;
                AmmoLeft = 0;
            }
            else
            {
                if (AmmoLeft > maxRounds - rounds)
                {
                    AmmoLeft -= maxRounds - rounds;
                    rounds = maxRounds;
                }
                else
                {
                    rounds += AmmoLeft;
                    AmmoLeft = 0;
                }
            }
            Debug.Log($"Reload {rounds} Rounds");
            return;
        }
    }

    public bool CanReload()
    {
        if (AmmoLeft > 0)
            return true;
        else
        {
            Debug.Log("No More Ammo Left");
            return false;
        }
    }

    public bool GunAttack()
    {
        if (rounds > 0)
        {
            rounds--;
            // Shoot Animation and Sound
            if (fireEffect != null)
                fireEffect.GunFireEffect();
            return true;
        }
        else
        {
            // no rounds Animation and Sounds
            return false;
        }
    }
}
