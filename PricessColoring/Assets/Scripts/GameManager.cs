//using SettingUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;
    public bool isTest;
    public bool showHandMenu = true;
    public bool showLog;
    string keyListGameContinue = "keyListGameContinue";
    public List<NameMinigame> listGameContinue = new List<NameMinigame>();
    private void Awake()
    {
        // Đảm bảo rằng chỉ có một thể hiện duy nhất tồn tại
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có một thể hiện duy nhất tồn tại, hủy bỏ thể hiện hiện tại
            Destroy(gameObject);
        }
    }

    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = (GameManager)FindObjectOfType(typeof(GameManager));
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.name = "GameManager";
                _instance = go.AddComponent<GameManager>();
            }
        }
        return _instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        showHandMenu = true;
        //if (!PlayerPrefs.HasKey(ConstantSetting.MusicEnable))
        //    PlayerPrefs.SetInt(ConstantSetting.MusicEnable, 1);
        //if (!PlayerPrefs.HasKey(ConstantSetting.SoundEnable))
        //    PlayerPrefs.SetInt(ConstantSetting.SoundEnable, 1);
        //if (!PlayerPrefs.HasKey(ConstantSetting.VibrateEnable))
        //    PlayerPrefs.SetInt(ConstantSetting.VibrateEnable, 1);
        //Vibration.Init();

        //Debug.LogError("fps: " + Application.targetFrameRate);
        Application.targetFrameRate = 60;
        //Debug.LogError("fps 2: " + Application.targetFrameRate);

        LoadListGameContinue();
    }

    public bool CheckInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Device is not connected to the network.");
            return false;
        }
        else
        {
            Debug.Log("Device is connected to the network.");
            return true;
        }
    }

    public  bool IsPointerOverUIObject()
    {
        // Kiểm tra xem có sự kiện pointer đang được thực hiện
        if (EventSystem.current == null)
            return false;

        // Kiểm tra sự kiện pointer của người dùng
        if (Application.isMobilePlatform)
        {
            // Dành cho các thiết bị cảm ứng như Android và iOS
            int touchCount = Input.touchCount;
            for (int i = 0; i < touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;
            }
        }
        else
        {
            // Dành cho máy tính và các thiết bị khác
            if (EventSystem.current.IsPointerOverGameObject())
                return true;
        }

        return false;
    }

    public float ScreenHightDp()
    {
        // Lấy kích thước màn hình của thiết bị (pixels)
        float screenWidthPixels = Screen.width;
        float screenHeightPixels = Screen.height;

        // Lấy mật độ điểm ảnh của thiết bị
        float screenDpi = Screen.dpi;

        // Tính toán chiều cao màn hình theo đơn vị dp
        float screenHeightDp = screenHeightPixels / (screenDpi / 160f);

        //Debug.Log("Screen height in dp: " + screenHeightDp);
        return screenHeightDp;
    }

    public void AddContinueGame(NameMinigame nameMinigame)
    {
        if(listGameContinue.Contains(nameMinigame))
            return;
        listGameContinue.Add(nameMinigame);
        SaveListGameContinue();
    }    

    public void SaveListGameContinue()
    {
        // Convert list enum to list string
        List<string> stringList = listGameContinue.Select(g => g.ToString()).ToList();

        // Serialize
        string json = JsonUtility.ToJson(new StringListWrapper { list = stringList });
        PlayerPrefs.SetString(keyListGameContinue, json);
        PlayerPrefs.Save();
    }

    public void LoadListGameContinue()
    {
        if (PlayerPrefs.HasKey(keyListGameContinue))
        {
            string json = PlayerPrefs.GetString(keyListGameContinue);
            StringListWrapper wrapper = JsonUtility.FromJson<StringListWrapper>(json);

            listGameContinue = wrapper.list
                .Select(s => Enum.TryParse(s, out NameMinigame result) ? result : default)
                .ToList();
        }
    }
}

[System.Serializable]
public class StringListWrapper
{
    public List<string> list;
}
