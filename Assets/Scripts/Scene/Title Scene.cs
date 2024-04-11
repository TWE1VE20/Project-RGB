using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [Header("UI")]
    [SerializeField] Image Shade;

    [Header("GameObjects")]
    [SerializeField] GameObject LobbyIdle;
    [SerializeField] Camera mainCamera;

    [Header("Light")]
    [SerializeField] Light FloorLight;
    [SerializeField] Light[] WallLight;
    [SerializeField] Light[] CellingLight;

    private void Start()
    {
        StartCoroutine(FirstOpening());
    }

    private void Update()
    {
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
        mainCamera.fieldOfView = 80;
        FloorLight.intensity = 0;
        foreach (Light light in WallLight)
            light.intensity = 0;
        foreach (Light light in CellingLight)
            light.intensity = 0;
        yield return new WaitForSeconds(2);
        Shade.color = new Color(Shade.color.r, Shade.color.g, Shade.color.b, 1f);
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 3)
        {
            float alpha = Mathf.Lerp(0f, 1f, t);
            Shade.color = new Color(Shade.color.r, Shade.color.g, Shade.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        for (float t = 1f; t > 0f; t -= Time.deltaTime / 3)
        {
            float FOV = Mathf.Lerp(40f, 80f, t);
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
    }
}
