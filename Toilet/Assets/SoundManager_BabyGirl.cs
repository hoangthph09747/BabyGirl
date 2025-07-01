using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SettingUI;
//using DG.Tweening;

public class SoundManager_BabyGirl : MonoBehaviour
{
    public static SoundManager_BabyGirl _instance;
    public AudioSource bgSource, fxSource;

    public static SoundManager_BabyGirl Instance
    {
        get
        {
            // Nếu chưa có thể hiện, hãy tạo một thể hiện mới
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager_BabyGirl>();

                // Nếu không tìm thấy trong scene, tạo một GameObject mới chứa thể hiện
                if (_instance == null)
                {
                    GameObject singletonObject = Instantiate(Resources.Load<GameObject>("SoundManager"));
                    _instance = singletonObject.GetComponent<SoundManager_BabyGirl>();
                }
            }

            // Trả về thể hiện duy nhất của class
            return _instance;
        }
    }

    private void Awake()
    {
        // Đảm bảo rằng chỉ có một thể hiện duy nhất tồn tại
        if (_instance == null)
        {
            _instance = this;
          //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có một thể hiện duy nhất tồn tại, hủy bỏ thể hiện hiện tại
           // Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fxSource.loop = false;
        bgSource.loop = true;
        bgSource.volume = 0.5f;
    }

    /// <summary>
    /// hàm chạy một âm đơn
    /// </summary>
    /// <param name="pathAudioClip"> đường dẫn tới file âm thanh trong thư mục resources của bạn</param>
    public void PlayOneShot(string pathAudioClip, float volume = 1)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            if (Resources.Load<AudioClip>(pathAudioClip))
            {
                fxSource.PlayOneShot(Resources.Load<AudioClip>(pathAudioClip), volume);
            }
            else
            {
                Debug.LogError("null: " + pathAudioClip);
            }
        }
    }

    public void PlayOneShotLong(string pathAudioClip)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            fxSource.Stop();
            if (Resources.Load<AudioClip>(pathAudioClip))
            {
                fxSource.clip = (Resources.Load<AudioClip>(pathAudioClip));
                fxSource.Play();
            }
            else
            {
                Debug.LogError("null: " + pathAudioClip);
            }
        }
    }

    public void PlayOneShotLong(AudioClip audioClip)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            fxSource.Stop();
            if (audioClip)
            {
                fxSource.clip = audioClip;
                fxSource.Play();
            }
            else
            {
                Debug.LogError("null audioclip");
            }
        }
    }

    public void PlayOneShot(AudioClip clip, float volume = 1)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            //fxSource.Stop();
            //fxSource.loop = false;
            if (clip != null)
            {
                //fxSource.clip = clip;
                //fxSource.Play();

                fxSource.PlayOneShot(clip, volume);
            }
            else
            {
                Debug.LogError("null audio clip");
            }
        }
    }

    /// <summary>
    /// true là nhạc nền bật
    /// </summary>
    /// <returns></returns>
    public bool CheckBgSoundOn()
    {
        //return PlayerPrefs.GetInt(ConstantSetting.MusicEnable, 1) == 1;
        return true; // Tạm thời trả về true, có thể thay đổi sau này nếu cần thiết
    }
    
    /// <summary>
    /// true là sound fx bật
    /// </summary>
    /// <returns></returns>
    public bool CheckSoundFXOn()
    {
        //return PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1;
        return true; // Tạm thời trả về true, có thể thay đổi sau này nếu cần thiết
    }    

    /// <summary>
    /// chạy một âm thanh background
    /// </summary>
    /// <param name="pathAudioClip"> đường dẫn tới file âm thanh background của bạn trong resources</param>
    public void PlayBgSound(string pathAudioClip, float volume = 0.5f, bool loop = true)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.MusicEnable, 1) == 1)
        {
            if (Resources.Load<AudioClip>(pathAudioClip))
            {
                bgSource.clip = Resources.Load<AudioClip>(pathAudioClip);
                bgSource.loop = loop;
                bgSource.volume = volume;
                bgSource.Play();

                this.volumeBG = volume;
                this.audioClipBG = bgSource.clip;
                this.isPlayingBg = true;
                this.loopBG = loop;
            }
            else
            {
                Debug.LogError("null: " + pathAudioClip);
            }
        }
    }

    public float volumeBG = 0.5f;
    public AudioClip audioClipBG;
    public bool isPlayingBg = false;
    public bool loopBG = true;
    /// <summary>
    /// chạy một âm thanh background
    /// </summary>
    /// <param name="audioClip"> </param>
    public void PlayBgSound(AudioClip audioClip, float volume = 0.5f, bool loop = true)
    {
        this.volumeBG = volume;
        this.audioClipBG = audioClip;
        //if (PlayerPrefs.GetInt(ConstantSetting.MusicEnable, 1) == 1)
        {
            if (audioClip != null)
            {
                bgSource.clip = audioClip;
                bgSource.loop = loop;
                bgSource.volume = volume;
                bgSource.Play();
                isPlayingBg = true;
                loopBG = loop;
            }
            else
            {
                Debug.LogError("null audioclip ");
            }
        }
    }

    public void PauseSoundBg()
    {
        /*bgSource.DOFade(0, 0.5f).OnComplete(() =>
        {
            bgSource.Pause();
            bgSource.volume = volumeBG;
            
        });*/ bgSource.Pause();
        bgSource.volume = volumeBG;
        isPlayingBg = false;
        //bgSource.Pause();
    }

    public void ResumeSoundBg()
    {
        isPlayingBg = true;
        bgSource.Play();
    }

    public void ReplaySoundBg()
    {
     //   StartCoroutine(RestoreBGM());
    }

   /* IEnumerator RestoreBGM()
    {
        //resume audio
        AudioListener.pause = false;
        AudioListener.volume = 1f;
        //reset audio settings
        AudioSettings.Reset(AudioSettings.GetConfiguration());
        yield return new WaitForSeconds(0.5f); // đợi 0.5s để đảm bảo audio đã reset xong
        //chạy lại nhạc nền nếu trước đó nó đang chạy
        if (isPlayingBg)
        {
            bgSource.clip = audioClipBG;
            bgSource.loop = loopBG;
            bgSource.volume = volumeBG;
            bgSource.Play();
        }
    }*/

  /*  void ResumeAndResetAudioSettings()
    {
        //resume audio
        AudioListener.pause = false;
        AudioListener.volume = 1f;
        //reset audio settings
        AudioSettings.Reset(AudioSettings.GetConfiguration());
    }*/

    /* ---Hiephm---*/

    public void PlayLoopSound(AudioSource audioSource, AudioClip audioClip, float volume = 0.5f, bool loop = true)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.MusicEnable, 1) == 1)
        {
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public void ClearSoundEffect()
    {
        fxSource.clip = null;
        fxSource.Stop();
    }
    
    /*---------*/

    public void PlaySoundEffect(string pathAudioClip, bool loop = false)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            fxSource.Stop();
            if (Resources.Load<AudioClip>(pathAudioClip))
            {
                fxSource.clip = (Resources.Load<AudioClip>(pathAudioClip));
                fxSource.loop = loop;
                fxSource.Play();
            }
            else
            {
                Debug.LogError("null: " + pathAudioClip);
            }
        }
    }

    public void StopSoundEffect()
    {
        fxSource.Stop();
    }

    public void PlaySoundEffectOneShot(AudioClip clip, float volume = 1,bool loop=false)
    {
        //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
        {
            if (clip != null)
            {
                fxSource.loop = loop;
                fxSource.PlayOneShot(clip, volume);
            }
            else
            {
                Debug.LogError("null audio clip");
            }
        }
    }


    public void DoMove(Transform trans,Vector3 posTo,float t,Action act=null)
    {
        StartCoroutine(MoveToPosition(trans,posTo, t,act));
    }

    private IEnumerator MoveToPosition(Transform trans,Vector3 targetPosition, float duration, Action act = null)
    {
        Vector3 startPosition = trans.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            trans.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        trans.position = targetPosition; // Đảm bảo dừng chính xác
        act?.Invoke();
    }
}