using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUIRotation : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] WeaponHolder weaponHolder;
    [SerializeField] RectTransform colorRotationUI;
    [SerializeField] GameObject[] Icons;

    [Header("Spec")]
    [SerializeField] float rotationLerpSpeed;

    private Weapons.Colors rotationCurColor;
    private float targetRotation;
    private float curRotation;

    private int currentWeapon;

    private void Start()
    {
        rotationCurColor = Weapons.Colors.RED;
        targetRotation = Rotation(Weapons.Colors.RED);
        curRotation = targetRotation;
        currentWeapon = weaponHolder.current;
        colorRotationUI.rotation = Quaternion.Euler(0, 0, curRotation);
    }

    private void Update()
    {
        if(currentWeapon != weaponHolder.current)
        {
            currentWeapon = weaponHolder.current;
            for(int i = 0; i < Icons.Length; i++)
            {
                if (i == currentWeapon)
                    Icons[i].SetActive(true);
                else
                    Icons[i].SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if(rotationCurColor != weaponHolder.weaponsList[weaponHolder.current].colorState)
        {
            rotationCurColor = weaponHolder.weaponsList[weaponHolder.current].colorState;
            targetRotation = Rotation(rotationCurColor);
        }
        if (targetRotation != curRotation)
        {
            curRotation = Mathf.Lerp(curRotation, targetRotation, rotationLerpSpeed * Time.fixedDeltaTime);
            colorRotationUI.rotation = Quaternion.Euler(0, 0, curRotation);
        }
    }

    private int Rotation(Weapons.Colors color)
    {
        switch(color)
        {
            case Weapons.Colors.RED:
                return 60;
            case Weapons.Colors.GREEN:
                return -60;
            case Weapons.Colors.BLUE:
                return 180;
        }
        return 60;
    }
}
