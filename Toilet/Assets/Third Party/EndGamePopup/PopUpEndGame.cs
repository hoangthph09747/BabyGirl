//using DG.Tweening;
//using DG.Tweening.Core;
//using DG.Tweening.Plugins.Options;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HongQuan
{
    public class PopUpEndGame : MonoBehaviour
    {
        public bool canClick;
        public UnityEvent onClickReplay;
        public UnityEvent onClickNext;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject mainPopup;
        [SerializeField] private SkeletonGraphic anim;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Image winLoseTextImage;

        [SerializeField] private Sprite[] winTextSprites;
        [SerializeField] private Sprite[] loseTextSprites;

        public void Replay()
        {
            if (!canClick) return;
            canClick = false;
            onClickReplay?.Invoke();
            Hide();
        }

        public void Next()
        {
            if (!canClick) return;
            canClick = false;
            onClickNext?.Invoke();
            Hide();
        }

        public void ShowWinGame()
        {
            Show();
            winLoseTextImage.sprite = winTextSprites[Random.Range(0, winTextSprites.Length - 1)];
            ScaleWinLoseImage();
            ShowAllButton();
        }

        public void ShowLoseGame()
        {
            Show();
            winLoseTextImage.sprite = loseTextSprites[Random.Range(0, loseTextSprites.Length - 1)];
            ScaleWinLoseImage();
            HideNextButton();
        }

        private void ScaleWinLoseImage()
        {
            winLoseTextImage.SetNativeSize();
            float xImageSize = winLoseTextImage.rectTransform.rect.width;
            float xParentSize = (winLoseTextImage.rectTransform.parent as RectTransform).rect.width;

            if (xParentSize >= xImageSize) return;

            float scaleFactor = xParentSize / xImageSize;
            winLoseTextImage.rectTransform.sizeDelta *= scaleFactor;
        }

        private void Show()
        {
            mainPopup.SetActive(true);
            canvasGroup.alpha = 0;
          //  canvasGroup.DOFade(1f, 1f);
            canClick = true;
           // anim.PlaySequanceAnimtions("showup", "idle");
        }

        public void Hide()
        {
            canvasGroup.alpha = 1;
            ShowAllButton();
            mainPopup.SetActive(false);
           /* canvasGroup.DOFade(0f, 1f).OnComplete(() =>
            {
                ShowAllButton();
                mainPopup.SetActive(false);
            });*/
        }

        public void HideNextButton()
        {
            nextButton.gameObject.SetActive(false);
            (replayButton.transform as RectTransform).anchoredPosition = new Vector2(0, (replayButton.transform as RectTransform).anchoredPosition.y);
        }

        public void ShowAllButton()
        {
            nextButton.gameObject.SetActive(true);
            replayButton.gameObject.SetActive(true);
            nextButton.image.rectTransform.anchoredPosition = new Vector2(96.7f, nextButton.image.rectTransform.anchoredPosition.y);
            replayButton.image.rectTransform.anchoredPosition = new Vector2(-96.7f, nextButton.image.rectTransform.anchoredPosition.y);
        }
    }
}
