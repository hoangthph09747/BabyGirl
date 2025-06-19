using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using QuanUtilities;
using System;
using HongQuan;

public class Transition : MonoBehaviour
{
    [SerializeField] private Image fullScreenImage;
    [SerializeField] private Image falldownImage;
    [SerializeField] private RawImage animatedImage;

    //Loading
    [SerializeField] private Sprite loadingSpriteBG;
    [SerializeField] CanvasGroup loadingCanvasGroup;

    public void FullScreen(float enterDuration, float stayDuration, float outDuraion, UnityAction onStay, UnityAction onOut, Color color = default)
    {
        fullScreenImage.gameObject.SetActive(true);
        if (color == default) color = Color.white;
        color.a = 0;
        fullScreenImage.color = color;
        fullScreenImage.DOFade(1, enterDuration).OnComplete(() =>
        {
            onStay?.Invoke();
            this.DelayFunction(stayDuration, () =>
            {
                fullScreenImage.DOFade(0, outDuraion).OnComplete(() =>
                {
                    onOut?.Invoke();
                    fullScreenImage.gameObject.SetActive(false);
                    fullScreenImage.sprite = null;
                });
            });
        });
    }

    public void FullScreenUntil(IEnumerator enumerator, Action onDone = null, Action onOut = null, float inDuration = 1, float stayDuration = 1, float outDuration = 1)
    {
        fullScreenImage.gameObject.SetActive(true);
        fullScreenImage.color = new Color(1, 1, 1, 0);
        fullScreenImage.DOFade(1, inDuration).OnComplete(() =>
        {
            var corotine = StartCoroutine(enumerator);
            this.WaitUntil(enumerator, () =>
            {
                fullScreenImage.DOFade(0, outDuration).OnComplete(() =>
                {
                    fullScreenImage.gameObject.SetActive(false);
                    fullScreenImage.sprite = null;
                    onDone?.Invoke();
                }).SetDelay(stayDuration);
                onOut?.Invoke();
            });
        });
    }

    public void FullScreen(UnityAction onStay, UnityAction onOut)
    {
        FullScreen(1, 0.5f, 1, onStay, onOut);
    }
    public void FullScreen(UnityAction onStay, UnityAction onOut, Color color)
    {
        FullScreen(1, 0.5f, 1, onStay, onOut, color);
    }

    public void FullScreen(UnityAction onStay, UnityAction onOut, Sprite sprite)
    {
        fullScreenImage.sprite = sprite;
        FullScreen(1, 0.5f, 1, onStay, onOut, Color.white);
    }

    public void Circle(float enterDuration, float stayDuration, float outDuraion, UnityAction onStay, UnityAction onOut)
    {

    }

    public void FallDown(Sprite image, float enterDuration, float stayDuration, float outDuraion, Vector3 scale, UnityAction onStay, UnityAction onOut)
    {
        falldownImage.gameObject.SetActive(true);
        falldownImage.rectTransform.localScale = scale;
        falldownImage.sprite = image;

        falldownImage.rectTransform.sizeDelta = ((RectTransform)falldownImage.rectTransform.parent).rect.size;

        falldownImage.rectTransform.anchoredPosition = new Vector2(0, falldownImage.rectTransform.sizeDelta.y * scale.y);

        Sequence fallDownSequence = DOTween.Sequence();
        fallDownSequence.Append(falldownImage.rectTransform.DOMoveY(0, enterDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            onStay?.Invoke();

        }));
        fallDownSequence.Append(falldownImage.rectTransform.DOMoveY(-falldownImage.rectTransform.sizeDelta.y * scale.y, outDuraion).SetEase(Ease.Linear).OnComplete(() =>
        {
            onOut?.Invoke();
            falldownImage.sprite = null;
            falldownImage.rectTransform.gameObject.SetActive(false);
        }));
    }

    public void FallDown(Sprite image, UnityAction onStay, UnityAction onOut)
    {
        FallDown(image, 2f, 0, 2f, new Vector3(1, 2, 1), onStay, onOut);
    }

    public void Animated(Texture2D texture, float enterDuration, float stayDuration, float outDuraion, UnityAction onStay, UnityAction onOut, Color color = default)
    {
        animatedImage.gameObject.SetActive(true);
        animatedImage.texture = texture;
        animatedImage.GetComponent<ScaleRawImageWithCanvas>().FitWithCavas();
        if (color == default) color = Color.white;
        color.a = 0;
        animatedImage.color = color;
        animatedImage.DOFade(1, enterDuration).OnComplete(() =>
        {
            onStay?.Invoke();
            this.DelayFunction(stayDuration, () =>
            {
                animatedImage.DOFade(0, outDuraion).OnComplete(() =>
                {
                    onOut?.Invoke();
                    animatedImage.gameObject.SetActive(false);
                });
            });
        });
    }

    public void Animated(Texture2D texture, UnityAction onStay, UnityAction onOut)
    {
        Animated(texture, 1, 1, 1, onStay, onOut);
    }

    bool isDoneLoading = false;
    float loadingFadeDuraion;
    Action onDoneLoading;
    public void Loading(ref Action<float> onLoading, Action onStay = null, Action onDone = null, float fadeDuration = 1)
    {
        isDoneLoading = false;
        onLoading += LoadingHandler;
        onDoneLoading = onDone;
        loadingFadeDuraion = fadeDuration;

        loadingCanvasGroup.gameObject.SetActive(true);
        fullScreenImage.gameObject.SetActive(true);
        fullScreenImage.color = new Color(1, 1, 1, 0);
        fullScreenImage.sprite = loadingSpriteBG;
        fullScreenImage.DOFade(1, loadingFadeDuraion);
        loadingCanvasGroup.alpha = 0;
        loadingCanvasGroup.DOFade(1, loadingFadeDuraion).OnComplete(() =>
        {
            onStay?.Invoke();
        });
    }

    private void LoadingHandler(float percentage)
    {
        if (isDoneLoading) return;

        if (percentage >= 1)
        {
            isDoneLoading = true;
            onDoneLoading?.Invoke();
            onDoneLoading = null;
            fullScreenImage.DOFade(0, loadingFadeDuraion);
            loadingCanvasGroup.DOFade(0, loadingFadeDuraion).OnComplete(() =>
            {
                onDoneLoading?.Invoke();

                fullScreenImage.sprite = null;
                fullScreenImage.gameObject.SetActive(false);
                loadingCanvasGroup.gameObject.SetActive(false);
            });
        }
    }
}
