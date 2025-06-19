using DG.Tweening;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizPrincessDream : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation mainAnim;

        [SerializeField, SpineAnimation] string dreamOn;
        [SerializeField, SpineAnimation] string dreamIdle;
        [SerializeField, SpineAnimation] string wakeup;
        [SerializeField, SpineAnimation] string startReality;
        [SerializeField, SpineAnimation] string reality;
        [SerializeField, SpineAnimation] string idle;

        [SerializeField] RichDream richDream;

        System.Action onDone;
        Camera mainCam;

        private void Awake()
        {
            mainCam = Camera.main;
        }

        private void Start()
        {
            if(!mainCam)
            mainCam = Camera.main; 
        }

        public void Dream(System.Action onDone)
        {
            this.onDone = onDone;
            mainAnim.PlaySequanceAnimations(dreamOn, dreamIdle);
            mainCam.transform.DOMove(transform.position + new Vector3(0, 0, -10) + new Vector3(0, 2), 1);
            mainCam.DOOrthoSize(4, 1);

            this.DelayFunction(mainAnim.GetAnimationDuration(dreamOn), () =>
            {
                SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/girl laugh");
                richDream.TurnOn();
                //this.DelayFunction(3, () => richDream.TurnOff(WakeupToReality));
                this.DelayFunction(3, () =>
                {
                    richDream.TurnOff(() =>
                    {
                        mainAnim.PlayAnimation(idle, true, .5f);
                        this.DelayFunction(1, onDone);

                        mainCam.transform.DOMove(new Vector3(0, 0, -10), 1);
                        mainCam.DOOrthoSize(PinQuizManager.instance.camereaOrthographicSize, 1);
                    });
                });
            });
        }

        private void WakeupToReality()
        {
            SoundManager_BabyGirl.Instance.PlayOneShot("IceCreamSounds/Pop");
            mainAnim.PlayAnimation(wakeup, false, 0.2f);
            this.DelayFunction(mainAnim.GetAnimationDuration(wakeup), () =>
            {
                mainAnim.PlayAnimation(reality, true, 0.5f);
                SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/Sob");
                this.DelayFunction(mainAnim.GetAnimationDuration(reality) * 3, () =>
                {
                    mainAnim.PlayAnimation(idle, true, .5f);
                    this.DelayFunction(1, onDone);

                    mainCam.transform.DOMove(new Vector3(0, 0, -10), 1);
                    mainCam.DOOrthoSize(PinQuizManager.instance.camereaOrthographicSize, 1);
                });
            });
        }
    }

}