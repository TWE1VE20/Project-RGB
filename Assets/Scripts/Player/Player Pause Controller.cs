using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPauseController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] AnalogGlitch[] analogGlitch;

    [Header("GameObjects")]
    [SerializeField] GameObject canvas;

    public void Paused()
    {
        canvas.SetActive(true);
        foreach(AnalogGlitch cameras in analogGlitch)
        {
            cameras.scanLineJitter = 0.3f;
            cameras.verticalJump = 0.1f;
            cameras.horizontalShake = 0.1f;
            cameras.colorDrift = 0.1f;
        }
    }

    public void PauseEnd()
    {
        canvas.SetActive(false);
        foreach (AnalogGlitch cameras in analogGlitch)
        {
            cameras.scanLineJitter = 0;
            cameras.verticalJump = 0;
            cameras.horizontalShake = 0;
            cameras.colorDrift = 0;
        }
    }
}
