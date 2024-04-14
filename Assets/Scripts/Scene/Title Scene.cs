using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [Header("UI")]
    [SerializeField] Image Shade;

    [Header("GameObjects")]
    [SerializeField] GameObject LobbyIdle;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject titleCanvas;
    [SerializeField] Image titleImage;
    [SerializeField] GameObject titleButtons;

    [Header("Light")]
    [SerializeField] Light FloorLight;
    [SerializeField] Light[] WallLight;
    [SerializeField] Light[] CellingLight;
    [SerializeField] float LightchangeSpeed;

    [Header("Sound")]
    [SerializeField] AudioSource audioSource;

    public bool skipIntro;

    private LightColor curColor;
    enum LightColor { R, G, B };

    private bool OpenEnd;
    private float lightchange;

    private void Start()
    {
        if (!skipIntro)
        {
            OpenEnd = false;
            curColor = LightColor.R;
            StartCoroutine(FirstOpening());
        }
        else
        {
            OpenEnd = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (!OpenEnd)
        {

        }
        else
        {
            // lightchange = Mathf.Lerp(lightchange, 1f, LightchangeSpeed * Time.deltaTime);
            lightchange += LightchangeSpeed * Time.deltaTime;
            if (lightchange >= 1f)
                lightchange = 1f;
            switch (curColor)
            {
                case LightColor.R:
                    foreach (Light light in CellingLight)
                        light.color = new Color(1f - lightchange, lightchange, 0);
                    FloorLight.color = new Color(1f - lightchange, lightchange, 0);
                    break;
                case LightColor.G:
                    foreach (Light light in CellingLight)
                        light.color = new Color(0, 1f - lightchange, lightchange);
                    FloorLight.color = new Color(0, 1f - lightchange, lightchange);
                    break;
                case LightColor.B:
                    foreach (Light light in CellingLight)
                        light.color = new Color(lightchange, 0, 1f - lightchange);
                    FloorLight.color = new Color(0, 1f - lightchange, lightchange);
                    break;
            }
            if (lightchange >= 1f)
            {
                lightchange = 0f;
                switch (curColor)
                {
                    case LightColor.R:
                        curColor = LightColor.G;
                        break;
                    case LightColor.G:
                        curColor = LightColor.B;
                        break;
                    case LightColor.B:
                        curColor = LightColor.R;
                        break;
                }
            }
        }
    }

    public void GameScene()
    {
        Manager.Scene.LoadScene("Game Scene");
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    IEnumerator FirstOpening()
    {
        Shade.gameObject.SetActive(true);
        LobbyIdle.SetActive(false);
        titleCanvas.gameObject.SetActive(false);
        titleButtons.SetActive(false);
        titleImage.fillAmount = 0;
        mainCamera.fieldOfView = 80;
        FloorLight.intensity = 0;
        foreach (Light light in WallLight)
            light.intensity = 0;
        foreach (Light light in CellingLight)
            light.intensity = 0.5f;
        Shade.color = new Color(Shade.color.r, Shade.color.g, Shade.color.b, 1f);
        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t);
            Shade.color = new Color(Shade.color.r, Shade.color.g, Shade.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 3)
        {
            float FOV = Mathf.Lerp(50f, 80f, t / 3);
            mainCamera.fieldOfView = FOV;
            yield return null;
        }
        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            float FOV = Mathf.Lerp(40f, 50f, t);
            mainCamera.fieldOfView = FOV;
            yield return null;
        }
        LobbyIdle.SetActive(true);
        yield return new WaitForSeconds(3);
        FloorLight.intensity = 1f;
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 3)
        {
            float FOV = Mathf.Lerp(30f, 40f, t);
            mainCamera.fieldOfView = FOV;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 3)
        {
            float FOV = Mathf.Lerp(60f, 30f, t);
            mainCamera.fieldOfView = FOV;
            yield return null;
        }
        foreach (Light light in WallLight)
            light.intensity = 0.5f;
        foreach (Light light in CellingLight)
            light.intensity = 1f;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        OpenEnd = true;
        titleCanvas.gameObject.SetActive(true);
        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            float fill = Mathf.Lerp(1, 0, t);
            titleImage.fillAmount = fill;
            yield return null;
        }
        titleButtons.SetActive(true);
    }
}
