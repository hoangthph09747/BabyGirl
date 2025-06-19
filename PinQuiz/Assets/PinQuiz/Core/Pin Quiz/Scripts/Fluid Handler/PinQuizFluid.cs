using PinQuiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizFluid : PinQuizEntity
    {
        public static bool isLowRes;
        [SerializeField] protected SpriteRenderer highResSprite;
        [SerializeField] protected SpriteRenderer lowResSprite;

        protected override void Start()
        {
            base.Start();

            if (isLowRes)
            {
                ChangeToLowResolution();
            }
        }

        public void ChangeToLowResolution()
        {
            highResSprite.enabled = false;
            lowResSprite.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (PinQuizManager.instance != null)
            {
                PinQuizManager.instance.fluids.Add(this);
            }
        }

        private void OnDisable()
        {
            if (PinQuizManager.instance != null)
            {
                PinQuizManager.instance.fluids.Remove(this);
            }
        }

    }
}



