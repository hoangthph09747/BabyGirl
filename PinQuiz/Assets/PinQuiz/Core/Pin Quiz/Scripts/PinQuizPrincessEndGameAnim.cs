using QuanUtilities;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizPrincessEndGameAnim : MonoBehaviour
    {
        //public MakeupGame.GirlCharectorMakeup makeUpGame;
        public Spine.Unity.SkeletonAnimation skeletonAnimation;

        [Spine.Unity.SpineAnimation] public string[] winAnims;
        //[Spine.Unity.SpineAnimation] public List<string> animsWithoutBag;

        private void Start()
        {
            //makeUpGame.ShowOnWinPinQuizGame(false);
        }

        public void RandomWinAnim()
        {
            skeletonAnimation.timeScale = 1.0f;
            string anim = winAnims.RandomElement();
            skeletonAnimation.PlayAnimation(anim, true);
            //makeUpGame.ShowOnWinPinQuizGame(!animsWithoutBag.Contains(anim));
            //makeUpGame.ShowOnWinPinQuizGame(true);
        }

        public void StopAnimation()
        {
            //skeletonAnimation.timeScale = 0;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RandomWinAnim();
            }
        }
#endif

    }
}
