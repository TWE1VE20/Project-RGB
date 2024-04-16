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

    [Header("Sounds")]
    [SerializeField] AudioSource titleAudioSource;
    [SerializeField] AudioClip titleTheme;
    [SerializeField] AudioClip[] introTheme;

    public bool skipIntro;

    private LightColor curColor;
    enum LightColor { R, G, B };

    private bool OpenEnd;
    private float lightchange;

    private void Start()
    {
        if (Manager.Scene.titleSkip)
            skipIntro = true;

        if (!skipIntro)
        {
            OpenEnd = false;
            curColor = LightColor.R;
            StartCoroutine(FirstOpening());
        }
        else
        {
            OpenEnd = true;
            titleAudioSource.loop = true;
            titleAudioSource.clip = titleTheme;
            titleAudioSource.Play();
        }
    }

    private void Update()
    {
        if (Manager.Scene.titleSkip)
            skipIntro = true;

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

    public void GameSceneLoad()
    {
        Manager.Scene.LoadScene("Game Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
        yield return new WaitForSeconds(2f);
    }

    IEnumerator FirstOpening()
    {
        Manager.Scene.titleSkip = true;
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
        titleAudioSource.loop = false;
        titleAudioSource.clip = introTheme[0];
        titleAudioSource.Play();
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
        titleAudioSource.clip = introTheme[1];
        titleAudioSource.Play();
        yield return new WaitForSeconds(3);
        titleAudioSource.clip = introTheme[2];
        titleAudioSource.Play();
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
        for(float t = 2f; t > 0f; t -= Time.deltaTime)
        {
            float lightIntensity = Mathf.Lerp(0.5f, 0f, t/2);
            foreach (Light light in WallLight)
                light.intensity = lightIntensity;
            yield return null;
        }
        titleAudioSource.clip = introTheme[3];
        titleAudioSource.Play();
        CellingLight[0].intensity = 1f;
        yield return new WaitForSeconds(0.7f);
        titleAudioSource.clip = introTheme[4];
        titleAudioSource.Play();
        CellingLight[1].intensity = 1f;
        yield return new WaitForSeconds(0.7f);
        titleAudioSource.clip = titleTheme;
        titleAudioSource.loop = true;
        titleAudioSource.Play();
        OpenEnd = true;
        titleCanvas.gameObject.SetActive(true);
        for (float t = 1f; t > 0f; t -= Time.deltaTime)
        {
            float fill = Mathf.Lerp(1, 0, t);
            titleImage.fillAmount = fill;
            yield return null;
        }
        Manager.Scene.titleSkip = true;
        titleButtons.SetActive(true);
    }
}
