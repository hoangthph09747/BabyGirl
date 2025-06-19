using DG.Tweening;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace HongQuan
{
    public class GameSoundManager : MonoBehaviour
    {
        public void PlayOneShot(string path)
        {
            //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) == 1)
            //{
            //    var clip = Resources.Load<AudioClip>(path);
            //    if (clip)
            //    {
            //        SoundManager_BabyGirl.Instance.fxSource.PlayOneShot(clip);
            //    }
            //    else
            //    {
            //        Debug.LogError("null: " + path);
            //    }
            //}
            SoundManager_BabyGirl.Instance.PlayOneShot(path);
        }
        public void PlayOneShot(AudioClip audioClip)
        {
            //if (PlayerPrefs.GetInt(ConstantSetting.SoundEnable, 1) != 1) return;
            SoundManager_BabyGirl.Instance.fxSource.PlayOneShot(audioClip);
        }

        public void PlayOneShot(string path, int loopTimes)
        {
            var delayAdd = Resources.Load<AudioClip>(path).length;
            float delay = 0;
            for (int i = 0; i < loopTimes; i++)
            {
                this.DelayFunction(delay, () => PlayOneShot(path));
                delay += delayAdd;
            }
        }

        public void PlayBackgroundMusic(string path)
        {
            SoundManager_BabyGirl.Instance.PlayBgSound(path, 0.5f, true);
        }

        public void FadeInMusic(string path, float fadeTime = 1)
        {
            var audioSource = SoundManager_BabyGirl.Instance.bgSource;
            SoundManager_BabyGirl.Instance.PlayBgSound(path, 0.5f, true);
            audioSource.volume = 0;
            DOVirtual.Float(0, 0.5f, fadeTime, value =>
            {
                audioSource.volume = value;
            });
        }

        public void FadeInMusic(AudioClip clip, float fadeTime = 1)
        {
            var audioSource = SoundManager_BabyGirl.Instance.bgSource;
            audioSource.volume = 0;
            audioSource.clip = clip;
            audioSource.Play();
            DOVirtual.Float(0, 0.5f, fadeTime, value =>
            {
                audioSource.volume = value;
            }).SetEase(Ease.Linear);
        }

        public void FadeOutMusic(TweenCallback onDone = null, float fadeTime = 1)
        {
            var audioSource = SoundManager_BabyGirl.Instance.bgSource;
            audioSource.volume = 0.5f;
            DOVirtual.Float(0.5f, 0, fadeTime, value =>
            {
                audioSource.volume = value;
            }).OnComplete(onDone).SetEase(Ease.Linear);
        }

        public void ChangeMusic(string path)
        {
            FadeOutMusic(() =>
            {
                FadeInMusic(path);
            });
        }
        public void ChangeMusic(AudioClip clip)
        {
            FadeOutMusic(() =>
            {
                FadeInMusic(clip);
            });
        }
        IEnumerator fadingMusicCorotine;
        public void PlayLoopFadingBackgroudMusic(string path, float fadeTime)
        {
            var clip = Resources.Load<AudioClip>(path);
            StopLoopFadingMusic();
            fadingMusicCorotine = LoopFading(path, clip.length, fadeTime);
            StartCoroutine(fadingMusicCorotine);
        }
        public void PlayLoopFadingBackgroudMusic(AudioClip clip, float fadeTime)
        {
            StopLoopFadingMusic();
            fadingMusicCorotine = LoopFading(clip, clip.length, fadeTime);
            StartCoroutine(fadingMusicCorotine);
        }
        IEnumerator LoopFading(string path, float audioDuration, float fadeTime)
        {
            FadeOutMusic(null, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            while (true)
            {
                FadeInMusic(path, fadeTime);
                yield return new WaitForSeconds(audioDuration - fadeTime);
                FadeOutMusic(null, fadeTime);
                yield return new WaitForSeconds(fadeTime);
            }
        }
        IEnumerator LoopFading(AudioClip clip, float audioDuration, float fadeTime)
        {
            FadeOutMusic(null, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            while (true)
            {
                FadeInMusic(clip, fadeTime);
                yield return new WaitForSeconds(audioDuration - fadeTime);
                FadeOutMusic(null, fadeTime);
                yield return new WaitForSeconds(fadeTime);
            }
        }

        public void StopLoopFadingMusic()
        {
            if (fadingMusicCorotine != null)
            {
                StopCoroutine(fadingMusicCorotine);
                fadingMusicCorotine = null;
            }
        }

        public void SetDefaultSoundValue()
        {
            SoundManager_BabyGirl.Instance.bgSource.volume = 0.5f;
            SoundManager_BabyGirl.Instance.fxSource.volume = 1;
        }

        public void SetVolumeFx(float value)
        {
            SoundManager_BabyGirl.Instance.fxSource.volume = value;
        }
        public void SetVolumeBGSound(float value)
        {
            SoundManager_BabyGirl.Instance.bgSource.volume = value;
        }
    }

}