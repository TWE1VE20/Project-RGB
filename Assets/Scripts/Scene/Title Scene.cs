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

    [Header("Light")]
    [SerializeField] Light FloorLight;
    [SerializeField] Light[] WallLight;
    [SerializeField] Light[] CellingLight;
    [SerializeField] float LightchangeSpeed;

    private LightColor curColor;
    enum LightColor { R, G, B };

    private bool OpenEnd;
    private float lightchange;

    private void Start()
    {
        OpenEnd = false;
        curColor = LightColor.R;
        StartCoroutine(FirstOpening());
    }

    private void Update()
    {
        if (!OpenEnd)
        {

        }
        else
        {
            switch (curColor)
            {
                case LightColor.R:
                    lightchange = Mathf.Lerp(lightchange, 1f, LightchangeSpeed * Time.deltaTime);
                    foreach (Light light in CellingLight)
                        light.color = new Color(1 - lightchange, lightchange, 0);
                    FloorLight.color = new Color(1 - lightchange, lightchange, 0);
                    break;
                case LightColor.G:
                    lightchange = Mathf.Lerp(lightchange, 1f, LightchangeSpeed * Time.deltaTime);
                    foreach (Light light in CellingLight)
                        light.color = new Color(0, 1 - lightchange, lightchange);
                    FloorLight.color = new Color(0, 1 - lightchange, lightchange);
                    break;
                case LightColor.B:
                    lightchange = Mathf.Lerp(lightchange, 1f, LightchangeSpeed * Time.deltaTime);
                    foreach (Light light in CellingLight)
                        light.color = new Color(lightchange, 0, 1 - lightchange);
                    FloorLight.color = new Color(0, 1 - lightchange, lightchange);
                    break;
            }
            if (lightchange >= 0.9f)
            {
                lightchange = 0;
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

    public void DemoScene()
    {
        // Manager.Scene.LoadScene("DemoScene");
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
        yield return new WaitForSeconds(1);
        OpenEnd = true;
    }
}
