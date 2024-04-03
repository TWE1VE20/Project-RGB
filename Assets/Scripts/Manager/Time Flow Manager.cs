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
    public float lerpSpeed = 1f;        // Lerp 속도
    public float targetTimeScale = 1f;  // 목표 시간 흐름 속도
    private float slowFactor;           // 현 TimeScale
    private bool Slow;                  // 슬로우모션하고있어야 하는지 유무

    // Real Time    실제 타임
    private int Rmin;
    private float Rsec;
    // In Game Time 인게임 타임
    private int Gmin;
    private float Gsec;
    // 타이머 시작 유무
    private bool timerON = false;

    private float fps;
    private float time;
    private int frame;

    

    void Start()
    {
        // 시간 관련
        slowFactor = 1f;
        RealTimer.text = string.Format("Real Time\t{0:D2}:{1:D2}", 0, 0);
        InGameTimer.text = string.Format("Game Time\t{0:D2}:{1:D2}", 0, 0);
        TimeScale.text = string.Format("Time Scale\t{0:f5}", Time.timeScale);
        FPSrate.text = string.Format("FPS \t{0:f1}", fps);

        Gsec = Time.time;
        Rsec = Time.time;

        // fps 관련
        fps = 0;
        time = 0;
        frame = 0;
        

        // 시간 정렬용 타이머
        StartCoroutine(WaitForSeconds());  // 타이머를 맞추기 위한 대기 코루틴
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

    // 시간 출력 함수
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

    // 슬로우모션 함수
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

    // 기다리는 코루틴
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1f);
        timerON = true;
    }
}
