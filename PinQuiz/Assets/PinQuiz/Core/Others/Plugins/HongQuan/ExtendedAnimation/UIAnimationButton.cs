using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ExtendedAnimation
{
    [RequireComponent(typeof(Button))]

    public class UIAnimationButton : MonoBehaviour
    {
        private Vector3 baseScale;
        private Button button;
        private Color baseColor;

        private void Awake()
        {
            baseScale = transform.localScale;
            button = GetComponent<Button>();
            button.onClick.AddListener(BounceAnimtion);
            baseColor = button.image.color;
        }

        public void BounceAnimtion()
        {
            transform.DOScale(baseScale * 0.8f, 0.1f);
            transform.DOScale(baseScale, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f);
        }

        public void Appear()
        {
            gameObject.SetActive(true);
            button.interactable = false;
            transform.localScale = baseScale + baseScale / 2;
            transform.DOScale(baseScale, 0.2f);
            DOVirtual.Color(new Color(baseColor.r, baseColor.g, baseColor.b, 0), baseColor, 0.2f, butColor =>
            {
                button.image.color = butColor;
            }).OnComplete(()=>button.interactable = true);
        }

        public void Disapear()
        {
            button.interactable = false;
            transform.localScale = baseScale;
            transform.DOScale(baseScale + baseScale / 2, 0.2f);
            DOVirtual.Color(baseColor, new Color(baseColor.r, baseColor.g, baseColor.b, 0), 0.2f, butColor =>
            {
                button.image.color = butColor;
            }).OnComplete(()=>gameObject.SetActive(false));
        }

    }
}
