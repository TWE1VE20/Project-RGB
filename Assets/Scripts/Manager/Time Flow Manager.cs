using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeFlowManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] TextMeshProUGUI RealTimer;
    [SerializeField] TextMeshProUGUI InGameTimer;
    [SerializeField] TextMeshProUGUI TimeScale;
    [SerializeField] TextMeshProUGUI FPSrate;

    [Header("SlowMotion")]
    public float lerpSpeed = 1f;        // Lerp �ӵ�
    public float targetTimeScale = 1f;  // ��ǥ �ð� �帧 �ӵ�
    private float slowFactor;           // �� TimeScale
    private bool Slow;                  // ���ο����ϰ��־�� �ϴ��� ����

    // Real Time    ���� Ÿ��
    private int Rmin;
    private float Rsec;
    // In Game Time �ΰ��� Ÿ��
    private int Gmin;
    private float Gsec;
    // Ÿ�̸� ���� ����
    private bool timerON = false;

    private float fps;
    private float time;
    private int frame;

    

    void Start()
    {
        // �ð� ����
        slowFactor = 1f;
        RealTimer.text = string.Format("Real Time\t{0:D2}:{1:D2}", 0, 0);
        InGameTimer.text = string.Format("Game Time\t{0:D2}:{1:D2}", 0, 0);
        TimeScale.text = string.Format("Time Scale\t{0:f5}", Time.timeScale);
        FPSrate.text = string.Format("FPS \t{0:f1}", fps);

        Gsec = Time.time;
        Rsec = Time.time;

        // fps ����
        fps = 0;
        time = 0;
        frame = 0;
        

        // �ð� ���Ŀ� Ÿ�̸�
        StartCoroutine(WaitForSeconds());  // Ÿ�̸Ӹ� ���߱� ���� ��� �ڷ�ƾ
    }

    void Update()
    {
        if(timerON)
            Timer();
        Fps();
    }
    private void FixedUpdate()
    {
        SlowMotion();
    }

    // �ð� ��� �Լ�
    private void Timer()
    {
        Rsec += Time.unscaledDeltaTime;
        Gsec += Time.deltaTime;
        if (Rsec >= 60f)
        {
            Rmin += 1;
            Rsec = 0;
        }
        if (Gsec >= 60f)
        {
            Gmin += 1;
            Gsec = 0;
        }
        RealTimer.text = string.Format("Real Time\t{0:D2}:{1:D2}", Rmin, (int)Rsec);
        InGameTimer.text = string.Format("Game Time\t{0:D2}:{1:D2}", Gmin, (int)Gsec);
        TimeScale.text = string.Format("Time Scale\t{0:f5}", Time.timeScale);
    }

    private void Fps()
    {
        if (Time.timeScale  >= 0.99f)
        {
            frame++;
            time += Time.deltaTime;
            if (time >= 1f)
            {
                fps = frame / time;
                frame = 0;
                time = 0;
            }
            FPSrate.text = string.Format("FPS \t{0:f1}", fps);
        }
        else
        {
            FPSrate.text = string.Format("FPS \t{0:f1} (Stoped)", fps);
        }
    }

    // ���ο��� �Լ�
    private void SlowMotion()
    {
        if (Slow)
        {
            if (Time.timeScale != targetTimeScale)
            {
                slowFactor = Mathf.Lerp(slowFactor, targetTimeScale, lerpSpeed * Time.fixedDeltaTime);
                Time.timeScale = slowFactor;
            }
        }
        else
        {
            if (Time.timeScale != 1)
            {
                slowFactor = Mathf.Lerp(slowFactor, 1f, lerpSpeed * Time.fixedDeltaTime);
                Time.timeScale = slowFactor;
            }
        }
    }

    private void OnSlowMotion(InputValue value)
    {
        if (value.isPressed)
            Slow = true;
        else
            Slow = false;
    }

    // ��ٸ��� �ڷ�ƾ
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1f);
        timerON = true;
    }
}
