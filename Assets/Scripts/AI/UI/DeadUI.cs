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
            // �������� ���� ���ҽ�ŵ�ϴ�.
            Color newColor = blackScreen.color;
            newColor.a += blackOutSpeed * Time.deltaTime;
            blackScreen.color = newColor;
            //�� ��
            yield return null;
        }
    }
}
