using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadUI : PopUpUI
{
    [SerializeField] Image blackScreen;
    [SerializeField] float blackOutSpeed;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(BlackOutCoroutine());
        //GetUI<Button>("ShotCutButton").onClick.AddListener(ShotCut);
        //GetUI<Button>("BackButton").onClick.AddListener(Close);
    }
    protected void OnEnable()
    {
        
    }
    protected void OnDisable()
    {
        StopAllCoroutines();
    }
    private void BlackOut()
    {
        StartCoroutine(BlackOutCoroutine());
    }
    IEnumerator BlackOutCoroutine()
    {
        while (blackScreen.color.a > 0.0f)
        {
            // 불투명도를 점차 감소시킵니다.
            Color newColor = blackScreen.color;
            newColor.a += blackOutSpeed * Time.deltaTime;
            blackScreen.color = newColor;
            //뭐 왜
            yield return null;
        }
    }
}
