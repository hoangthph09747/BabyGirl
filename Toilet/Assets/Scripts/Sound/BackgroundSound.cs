using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class BackgroundSound : MonoBehaviour
    {
        public void PlayBackgroundSound(string path)
        {
            SoundManager_BabyGirl.Instance.PlayBgSound(path);
        }

        public void PlayBackgroundSound(AudioClip sound)
        {
            //SoundManager_BabyGirl.Instance.PlayBgSound(sound);
        }

        public void ResumeBackgroundSound()
        {
            SoundManager_BabyGirl.Instance.ResumeSoundBg();
        }

        public void PauseBackgroundSound()
        {
            SoundManager_BabyGirl.Instance.PauseSoundBg();
        }
    }

}
