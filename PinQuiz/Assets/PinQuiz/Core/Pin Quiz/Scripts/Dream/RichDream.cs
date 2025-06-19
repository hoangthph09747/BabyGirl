using DG.Tweening;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class RichDream : MonoBehaviour
    {
        [SerializeField] ParticleSystem eff;
        [SerializeField] SkeletonAnimation anim;

        public void TurnOn()
        {
            gameObject.SetActive(true);

            var mainEff = eff.main;

            DOVirtual.Color(new Color(1, 1, 1, 0), Color.white, 1, color =>
            {
                anim.SetColor(color);
                mainEff.startColor = color;
            });
        }

        public void TurnOff(TweenCallback onComplete = null)
        {
            var mainEff = eff.main;
            onComplete += () => gameObject.SetActive(false);

            DOVirtual.Color(Color.white, new Color(1, 1, 1, 0), 1, color =>
            {
                anim.SetColor(color);
                mainEff.startColor = color;
            }).OnComplete(onComplete);
        }

    }

}