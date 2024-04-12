using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kino;

public class PlayerDeathController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] DigitalGlitch digitalGlitch1;
    [SerializeField] DigitalGlitch digitalGlitch2;

    [Header("GameObjects")]
    [SerializeField] GameObject canvas;
    [SerializeField] Image image;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject Buttons;

    [SerializeField, Range(0, 1)]
    float _intensity = 0;

    [SerializeField] bool canvasOn;
    [SerializeField] float LerpSpeed;
    [SerializeField] float CameraGlitchSpeed;

    private float alpha;
    private float intensityValue;

    public float intensity
    {
        get { return _intensity; }
        set { _intensity = value; }
    }

    private void Start()
    {
        canvas.SetActive(false);
        Buttons.SetActive(false);
        digitalGlitch1.intensity = this.intensity;
        digitalGlitch2.intensity = this.intensity;
        intensityValue = this.intensity;
    }

    private void Update()
    {
        if (!playerController.IsAlive)
        {
            if(Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            if (!canvas.activeSelf)
                canvas.SetActive(true);
            if (intensity != 1)
            {
                intensityValue += CameraGlitchSpeed * Time.deltaTime;
                digitalGlitch1.intensity = intensityValue;
                digitalGlitch2.intensity = intensityValue;
            }
            alpha = Mathf.Lerp(alpha, 1f, LerpSpeed * Time.deltaTime);
            image.color = new Color(1, 1, 1, alpha);
            if(alpha > 0.9f)
                Buttons.SetActive(true);
        }
    }

    private void Reset()
    {
        
    }
}
