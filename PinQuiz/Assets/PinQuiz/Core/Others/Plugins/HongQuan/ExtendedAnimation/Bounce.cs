using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ExtendedAnimation
{
    public class Bounce : MonoBehaviour
    {
        public bool playOnEnable;
        public bool playOnStart;

        public float delay = 0f;
        public float strength = 0.2f;
        public float duration = 0.2f;

        Vector3 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        private void OnEnable()
        {
            if(playOnEnable) PlayAnimation();
        }

        void Start()
        {
            if(playOnStart) PlayAnimation();
        }

        [ContextMenu("Bounce")]
        public void PlayAnimation()
        {
            transform.DOPunchScale(baseScale * strength, duration).OnComplete(()=>transform.DOScale(baseScale,0.2f)).SetDelay(delay);
        }
    }
}

