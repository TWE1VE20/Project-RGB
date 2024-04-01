using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapons
{
    [Header("Component")]
    [SerializeField] Animator animator;

    [Header("Gun Status")]
    [SerializeField] float timeforReload;
    [SerializeField] float timeforAttack;
    public int AmmoLeft;        // ³²Àº ÃÑ¾Ë °¹¼ö
    public int maxRounds;   // ÅºÃ¢ ÃÖ´ë Å©±â
    public int rounds { get; private set; }     // ÇöÁ¦ ÃÑ¿¡ µé¾îÀÖ´Â ÃÑ¾Ë °¹¼ö

    public Gun() : base()
    {
        this.attackType = Weapons.AttackType.GUN;
        this.rounds = maxRounds;
        SetReloadTime(timeforReload);
        SetAttackTime(timeforAttack);
    }

    private void Start()
    {
        base.Starting();
        this.attackType = Weapons.AttackType.GUN;
        this.rounds = maxRounds;
        SetReloadTime(timeforReload);
        SetAttackTime(timeforAttack);
    }

    public override bool Attack()
    {
        if(rounds > 0) 
        {
            rounds--;
            // Shoot Animation and Sound
            return true;
        }
        else
        {
            // no rounds Animation and Sounds
            return false;
        }
    }

    public override void Reload() 
    {
        if(AmmoLeft >= maxRounds)
        {
            AmmoLeft -= maxRounds - rounds;
            rounds = maxRounds;
            Debug.Log($"Reload {rounds} Rounds");
            return;
        }
        else if(AmmoLeft > 0)
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

    public override bool CanReload()
    {
        if (AmmoLeft > 0)
            return true;
        else
        {
            Debug.Log("No More Ammo Left");
            return false;
        }
    }
}
