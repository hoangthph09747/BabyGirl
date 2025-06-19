using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có một thể hiện duy nhất tồn tại, hủy bỏ thể hiện hiện tại
            Destroy(gameObject);
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
    public void PlayOneShot(string pathAudioClip)
    {

        if (Resources.Load<AudioClip>(pathAudioClip))
        {
            fxSource.PlayOneShot(Resources.Load<AudioClip>(pathAudioClip));
        }
        else
        {
            Debug.LogError("null: " + pathAudioClip);
        }

    }

    public void PlayOneShotLong(string pathAudioClip)
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

    public void PlayOneShotLong(AudioClip audioClip)
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

    public void PlayOneShot(AudioClip clip)
    {

        //fxSource.Stop();
        //fxSource.loop = false;
        if (clip != null)
        {
            //fxSource.clip = clip;
            //fxSource.Play();

            fxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("null audio clip");
        }

    }


    /// <summary>
    /// chạy một âm thanh background
    /// </summary>
    /// <param name="pathAudioClip"> đường dẫn tới file âm thanh background của bạn trong resources</param>
    public void PlayBgSound(string pathAudioClip, float volume = 0.5f, bool loop = true)
    {

        if (Resources.Load<AudioClip>(pathAudioClip))
        {
            bgSource.clip = Resources.Load<AudioClip>(pathAudioClip);
            bgSource.loop = loop;
            bgSource.volume = volume;
            bgSource.Play();
        }
        else
        {
            Debug.LogError("null: " + pathAudioClip);
        }

    }

    /// <summary>
    /// chạy một âm thanh background
    /// </summary>
    /// <param name="audioClip"> </param>
    public void PlayBgSound(AudioClip audioClip, float volume = 0.5f, bool loop = true)
    {

        if (audioClip != null)
        {
            bgSource.clip = audioClip;
            bgSource.loop = loop;
            bgSource.volume = volume;
            bgSource.Play();
        }
        else
        {
            Debug.LogError("null audioclip ");
        }

    }

    public void PauseSoundBg()
    {
        bgSource.Pause();
    }

    public void ResumeSoundBg()
    {
        bgSource.Play();
    }

    /* ---Hiephm---*/

    public void PlayLoopSound(AudioSource audioSource, AudioClip audioClip, float volume = 0.5f, bool loop = true)
    {

        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();

    }

    public void ClearSoundEffect()
    {
        fxSource.clip = null;
        fxSource.Stop();
    }

    /*---------*/

    public void PlaySoundEffect(string pathAudioClip, bool loop = false)
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

    public void StopSoundEffect()
    {
        fxSource.Stop();
    }

    public void PlaySoundEffectOneShot(AudioClip clip)
    {

        if (clip != null)
        {
            fxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("null audio clip");
        }

    }
}