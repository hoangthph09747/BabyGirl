using Luna.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaIntegrationManager : MonoBehaviour
{
    public static LunaIntegrationManager instance;
    [SerializeField] GameObject ScreenOver;
    //public string LinkStore;
    public bool OverTimeLimt;
    public float TimeLimit;
    private void Awake()
    {
        // Đăng ký sự kiện khi quảng cáo bị tạm dừng hoặc tiếp tục
        if (instance == null) instance = this;
        LifeCycle.OnPause += OnGamePaused;
        LifeCycle.OnResume += OnGameResumed;
    }
    private void Start()
    {
        StartCountdown(null);
    }
    public void OpenStore()
    {
       Debug.Log("Open Chplay");
      //  Application.OpenURL(LinkStore);
        Playable.InstallFullGame();
    }
    private void OnDestroy()
    {
        // Hủy đăng ký để tránh memory leak
       // LifeCycle.OnPause -= OnGamePaused;
        //LifeCycle.OnResume -= OnGameResumed;
    }

    private void OnGamePaused()
    {
     //   Debug.Log("Game Paused by Luna LifeCycle");
        Time.timeScale = 0f;
        // Tạm dừng âm thanh, hiệu ứng,...
    }

    private void OnGameResumed()
    {
       // Debug.Log("Game Resumed by Luna LifeCycle");
        Time.timeScale = 1f;
        // Khôi phục âm thanh, hiệu ứng,...
    }

    public void OnCTAClick()
    {
        //Debug.Log("CTA Clicked - Installing game...");
       // Playable.InstallFullGame(); // Mở app store
    }

    public void OnGameFinished()
    {
      //  EndCardController.Instance.OpenEndCard();
        ScreenOver.SetActive(true);
        //  Debug.Log("Game Finished - Notifying Luna...");
        LifeCycle.GameEnded(); // Thông báo đã kết thúc playable
    }


    public void StartCountdown(Action onComplete)
    {
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(Action onComplete)
    {
        float timer = 0f;
        TimeLimit += 1;
        while (timer < TimeLimit)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        OverTimeLimt = true;
        onComplete?.Invoke();
    }
    public void LogEventLvStart(int lv)
    {
       // Analytics.LogEvent(Analytics.EventType.LevelStart, lv);
        Analytics.LogEvent("level_start_"+ lv, lv);
    }
    public void LogEventLvAchieved(int lv)
    {
        Analytics.LogEvent("level_achieved_" + lv, lv);
    }
    public void LogEventlevelLose(int lv)
    {
        Analytics.LogEvent("level_lose_" + lv, lv);
    }
}
