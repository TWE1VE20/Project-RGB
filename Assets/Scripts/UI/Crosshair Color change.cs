using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColorchange : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] Image crosshairImage;
    [SerializeField] WeaponHolder weaponHolder;

    private void Start()
    {
        crosshairImage.color = new Color(255, 0, 0);
    }

    private void FixedUpdate()
    {
        switch(weaponHolder.weaponsList[weaponHolder.current].colorState)
        {
            case Weapons.Colors.RED:
                crosshairImage.color = new Color(255, 0, 0);
                break;
            case Weapons.Colors.BLUE:
                crosshairImage.color = new Color(0, 0, 255);
                break;
            case Weapons.Colors.GREEN:
                crosshairImage.color = new Color(0, 255, 0);
                break;
        }
    }
}
