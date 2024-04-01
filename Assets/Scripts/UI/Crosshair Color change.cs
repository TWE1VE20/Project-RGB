using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColorchange : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] Image crosshairImage;
    [SerializeField] WeaponHolder weaponHolder;

    private Weapons.Colors curColor;

    private void Start()
    {
        curColor = Weapons.Colors.RED;
    }

    private void Update()
    {
        if (curColor != weaponHolder.weaponsList[weaponHolder.current].colorState)
        {
            curColor = weaponHolder.weaponsList[weaponHolder.current].colorState;
        }
        switch(curColor)
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
