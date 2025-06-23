using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class PlaySound : MonoBehaviour
    {
        public void PlayOneShot(string path)
        {
            SoundManager_BabyGirl.Instance.PlayOneShot(path);
        }

        public void PlayOneShot(AudioClip clip)
        {
            Manager.instance.soundManager.PlayOneShot(clip);
        }
    }
}