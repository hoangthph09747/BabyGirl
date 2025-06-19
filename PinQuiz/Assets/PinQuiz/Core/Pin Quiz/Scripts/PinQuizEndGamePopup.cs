using DG.Tweening;
using HongQuan;
using QuanUtilities;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PinQuiz
{
    public class PinQuizEndGamePopup : MonoBehaviour
    {
        [SerializeField] SkeletonGraphic anim;
        [SerializeField] List<EndGameAnimData> endGameAnimData;
        [SerializeField, SpineSkin] string loseSkin;
        [SerializeField, SpineSkin] string[] winSkin;
        [SerializeField, SpineSkin] string slimeSkin;
        [SerializeField] GameObject stars;
        [SerializeField] ParticleSystem fireParticalEffect;
        [SerializeField] SkeletonGraphic monsterGetCoinAnim;

        [SerializeField] Button nextButton;
        [SerializeField] Button replayButton;
        [SerializeField] RectTransform board;

        [SerializeField] float animationDuration = 0.2f;

       // [SerializeField] AudioClip loseSound;

        public UnityEvent onClickNext;
        public UnityEvent onClickReplay;

        private Dictionary<EndGameType, string> endGameAnmations = new Dictionary<EndGameType, string>();
        private CanvasGroup canvasGroup;

        private Vector2 defaultAnimPos = new Vector2(0, -170);

        private EndGameType endGameType;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            foreach (var data in endGameAnimData)
                endGameAnmations.Add(data.type, data.anim);

            nextButton.onClick.AddListener(ClickNext);
            replayButton.onClick.AddListener(ClickReplay);
        }

        public void Show(EndGameType endGameType)
        {
          /*  gameObject.SetActive(true);

            this.endGameType = endGameType;

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.DOFade(1, animationDuration).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canClick = true;
            });
            DOVirtual.Color(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), animationDuration, color =>
            {
                if (anim.gameObject.activeSelf)
                    anim.color = color;
                if (monsterGetCoinAnim.gameObject.activeSelf)
                    monsterGetCoinAnim.color = color;
            });
            board.localScale = Vector3.one * 1.5f;
            PinQuizManager.instance.endPrincessAnim.transform.localScale = Vector3.zero;
            board.DOScale(Vector3.one, animationDuration).OnComplete(() =>
            {
                if (endGameType == EndGameType.Win)
                {
                    PinQuizManager.instance.endPrincessAnim.transform.position = anim.transform.position;
                    PinQuizManager.instance.endPrincessAnim.transform.DOScale(PinQuizManager.instance.winAnimLocalScale * 0.21f, 0.5f).SetEase(Ease.OutBack);
                }
            });

            anim.gameObject.SetActive(true);
            monsterGetCoinAnim.gameObject.SetActive(false);
            anim.rectTransform.anchoredPosition = defaultAnimPos;
            anim.PlayAnimation(endGameAnmations[endGameType], true);
            InitLoseButton();
            anim.SetSkin(loseSkin);
            stars.SetActive(false);

            switch (endGameType)
            {
                case EndGameType.Win:
                    stars.SetActive(true);
                    //anim.SetSkin(winSkin.RandomElement());
                    anim.gameObject.SetActive(false);
                    InitWinButtons();
                    SoundManager_BabyGirl.Instance.PlayOneShot("FishingSounds/girl laugh");
                    break;
                case EndGameType.Slime:
                    anim.SetSkin(loseSkin, slimeSkin);
                    SoundManager_BabyGirl.Instance.PlayOneShot(loseSound);
                    break;
                case EndGameType.Fire:
                    fireParticalEffect.Play();
                    SoundManager_BabyGirl.Instance.PlayOneShot(loseSound);
                    break;
                case EndGameType.MonsterGetCoins:
                    anim.gameObject.SetActive(false);
                    monsterGetCoinAnim.gameObject.SetActive(true);
                    SoundManager_BabyGirl.Instance.PlayOneShot(loseSound);
                    break;
                default:
                    SoundManager_BabyGirl.Instance.PlayOneShot(loseSound);
                    break;
            }*/
        }

        private void InitWinButtons()
        {
            nextButton.gameObject.SetActive(true);

            var replayRectTransform = replayButton.transform as RectTransform;
            var nextTransform = nextButton.transform as RectTransform;

            replayRectTransform.anchoredPosition = new Vector2(-90, replayRectTransform.anchoredPosition.y);
            nextTransform.anchoredPosition = new Vector2(90, nextTransform.anchoredPosition.y);
        }

        private void InitLoseButton()
        {
            nextButton.gameObject.SetActive(false);
            var replayRectTransform = replayButton.transform as RectTransform;
            replayRectTransform.anchoredPosition = new Vector2(0, replayRectTransform.anchoredPosition.y);
        }

        public void Close()
        {
            fireParticalEffect.Stop();

            canvasGroup.interactable = false;
            canvasGroup.DOFade(0, animationDuration);
            board.localScale = Vector3.one;
            board.DOScale(Vector3.one * 1.5f, animationDuration);

            DOVirtual.Color(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), animationDuration, color =>
            {
                if (anim.gameObject.activeSelf)
                    anim.color = color;
                if (monsterGetCoinAnim.gameObject.activeSelf)
                    monsterGetCoinAnim.color = color;
            }).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        bool canClick = false;
        private void ClickNext()
        {
            if (!canClick) return;
            canClick = false;

            Close();
            onClickNext?.Invoke();
        }

        private void ClickReplay()
        {
            if (!canClick) return;
            canClick = false;

            Close();
            onClickReplay?.Invoke();
        }

    }

    [System.Serializable]
    public struct EndGameAnimData
    {
        public EndGameType type;
        [SpineAnimation]
        public string anim;
    }

    [System.Serializable]
    public enum EndGameType
    {
        None = 0,
        Win = 1,
        Slime = 2,
        Bomb = 3,
        Bomb2 = 8,
        DogLick = 4,
        DogTakeShoe = 5,
        SlimeTouchTreasure = 6,
        CoinStuck = 7,
        Fire = 9,
        MonsterGetCoins = 10,
    }

}