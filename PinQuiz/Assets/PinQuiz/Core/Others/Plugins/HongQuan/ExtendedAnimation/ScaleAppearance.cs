using DG.Tweening;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedAnimation
{
    public class ScaleAppearance : MonoBehaviour
    {
        public float delay;

        Vector3 baseScale;
        private void Awake()
        {
            baseScale = transform.localScale;

        }

        private void OnEnable()
        {
            this.DelayFunction(Time.deltaTime, Appear);
        }

        public void Appear()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(baseScale, 1f).SetDelay(Random.Range(0.1f, 0.5f)).SetEase(Ease.OutBack).SetDelay(delay);
        }

        public void Disappear()
        {
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            //Disappear();
        }
    }
}
