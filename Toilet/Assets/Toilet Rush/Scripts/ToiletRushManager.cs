//using DG.Tweening;
using HongQuan;
using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
//using UnityEngine.AddressableAssets;
using UnityEngine.Events;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ToiletRush
{
    public class ToiletRushManager : MonoBehaviour
    {
        public static ToiletRushManager instance;

        [SerializeField] ToiletRushWinUI winUI;
        [SerializeField] ToiletRushLoseUI loseUI;
        [SerializeField] RectTransform replayButton;
        [SerializeField] RectTransform adsButton;
        [SerializeField] RectTransform ideaButton;
        // [SerializeField] TMP_Text levelText;
        [SerializeField] private ParticleSystem hitEff;
        [SerializeField] private ParticleSystem winEff;
        [SerializeField] Text levelText;
        // [SerializeField] TMP_Text hintCountText;
        [SerializeField] ToiletRushDrawLine drawLine;
        [SerializeField] ToiletRushLevel[] LevelPrefabs;
        [SerializeField] StringArray[] toiletAddresses;

        public int Level;
        public int Map;
        public bool EndGame;
        public Gender LoseGender;
        public ToiletRushLevel currentLevel;
        public Sprite[] nextToilet = new Sprite[2];
        public Sprite[] currentToilet = new Sprite[2];

        //public List<AsyncOperationHandle<GameObject>> objectAddressableHanlers = new List<AsyncOperationHandle<GameObject>>();
        //public List<AsyncOperationHandle<Sprite>> spriteAddressableHanlers = new List<AsyncOperationHandle<Sprite>>();
        public UnityEvent onGCAddressable;

        public const string CharacterTag = "Player";
        public const string GirlToiletTag = "Cat";
        public const string BoyToiletTag = "Cake";
        public const string ObstacleTag = "Obstacle";

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //try
            //{
            //    MyAdsBabyGirl.GetInstance().ShowBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
            //}
            //catch (System.Exception e)
            //{
            //    print(e.Message);
            //}
            //BonBonAdvertising.OnNoInternet += NoInternet;
            //BonBonAdvertising.OnAdsNoReady += OnAdsNoReady;
            Level = 0;
            LoadLevel();
            UpdateHintCountText();
            hitEff.Stop();
            //winEff.Stop();
        }

        private void LoadLevel()
        {
            //BonBonAdvertising.ShowInterstitialAd(LoadSceneManager.Instance.nameMinigame.ToString());
            //MyAnalytics.GetInstance().LogStartGame();
            /*Level = GetLevel()*/;
            //MyAnalytics.GetInstance().LogLevelGame(Level);
            // Debug.Log(Level);
            Map = Level / 3;
            levelText.text = "LEVEL " + (Level + 1).ToString();
            LunaIntegrationManager.instance.LogEventLvStart(Level + 1);
            string levelAddress = $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/Levels/Level {Level + 1}.prefab";

            if (Map == 0)
            {
                OffHintButton();
            }
            else
            {
                OnHintButton();
            }
            if (currentLevel != null)
            {
                Destroy(currentLevel.gameObject);
            }
            currentLevel = Instantiate(LevelPrefabs[Level]);
            /*AddressableManager.Load<GameObject>(levelAddress, handler =>
            {
                if (currentLevel != null)
                {
                    Destroy(currentLevel.gameObject);
                }
                objectAddressableHanlers.Add(handler);
                currentLevel = Instantiate(handler.Result).GetComponent<ToiletRushLevel>();
            });

            if (Map + 1 < 12)
            {
                string adr = $"Toilet Rush_Toilet_{Map + 1}_0";
                AddressableManager.Load<Sprite>(adr, handler =>
                {
                    spriteAddressableHanlers.Add(handler);
                    nextToilet[0] = handler.Result;
                });
                adr = $"Toilet Rush_Toilet_{Map + 1}_1";
                AddressableManager.Load<Sprite>(adr, handler =>
                {
                    spriteAddressableHanlers.Add(handler);
                    nextToilet[1] = handler.Result;
                });
            }
            if (Map < 12)
            {
                string adr = $"Toilet Rush_Toilet_{Map}_0";
                AddressableManager.Load<Sprite>(adr, handler =>
                {
                    spriteAddressableHanlers.Add(handler);
                    currentToilet[0] = handler.Result;
                });
                adr = $"Toilet Rush_Toilet_{Map}_1";
                AddressableManager.Load<Sprite>(adr, handler =>
                {
                    spriteAddressableHanlers.Add(handler);
                    currentToilet[1] = handler.Result;
                });
            }*/

            drawLine.ClearDraw();


        }
        private IEnumerator DelayedNextLevel(float delay)
        {
            yield return new WaitForSeconds(delay);
            NextLevel();
            winEff.Stop();
        }
        public void WinGame()
        {
            if (EndGame) return; EndGame = true;
            winEff.Play();
            SoundManager_BabyGirl.Instance.StopSoundEffect();
            winUI.Show();
            LunaIntegrationManager.instance.LogEventLvAchieved(Level + 1);
            // DOVirtual.DelayedCall(2,()=>{ NextLevel();});
            StartCoroutine(DelayedNextLevel(2));
           // MyAnalytics.GetInstance().LogWinGame();
        }
        public void showHitEff(Vector3 pos)
        {
            hitEff.transform.position = pos;
            hitEff.Play();
        }
        public void LoseGameForHit()
        {
            if (EndGame) return; EndGame = true;
            Level = -1;
            LunaIntegrationManager.instance.LogEventlevelLose(Level + 1);
            if (LunaIntegrationManager.instance.OverTimeLimt)
            {
                LunaIntegrationManager.instance.OnGameFinished();
                return;
            }

            StartCoroutine(DelayedNextLevel(2));
            //loseUI.ShowHit();
        }

        public void LoseForTouchBanana(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowBanana();
        }
        public void LoseForTouchGum(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowGum();
        }
        public void LoseForTouchBeetle(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowBeetle();
        }
        public void LoseForTouchCrab(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowCrab();
        }

        public void LoseForTouchSkateBoard(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowSkateBoard();
        }
        public void LoseForTouchSnowSkateBoard(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowSnowSkateBoard();
        }
        public void LoseForTouchCat(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowCat();
        }
        public void LoseForTouchPony(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowPony();
        }
        public void LoseForSpider(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowSpider();
        }
        public void LoseForTouchTomb(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowTomb();
        }
        public void LoseForTouchGhost(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowGhost();
        }
        public void LoseForTouchZombie(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowZombie();
        }
        public void LoseForTouchReindeer(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowReindeer();
        }
        public void LoseForSnowman(Gender gender)
        {
            if (EndGame) return; EndGame = true;
            LoseGender = gender;
            loseUI.ShowSnowman();
        }

        public void RestartGame()
        {
            LoadLevel();
        }

        public void NextLevel()
        {
            //SetLevel(Level + 1);
            // LoadMainGame();
            Level++;
            if (Level >= 3 || LunaIntegrationManager.instance.OverTimeLimt)
            {
                LunaIntegrationManager.instance.OnGameFinished();
                Level = 0;
                return;
            }    
            LoadLevel();
            EndGame = false;
        }

        public void AdsNextLevel()
        {
            //BonBonAdvertising.RewardedAdCompleted += OnDoneRewardAdsNextLevel;
            //BonBonAdvertising.RewardedAdSkipped += RemoveRewardNextLevelEvent;
            //PopupWatchVideoAds.OnHidePopupVideoAd += RemoveRewardNextLevelEvent;
            //PopupWatchVideoAds.instance.Show();
        }

        public void AdsSkipLevel()
        {
            //BonBonAdvertising.RewardedAdCompleted += OnDoneRewardAdsNextLevel;
            //BonBonAdvertising.RewardedAdSkipped += RemoveRewardNextLevelEvent;
            //BonBonAdvertising.ShowRewardedAd(LoadSceneManager.Instance.nameMinigame.ToString());
        }

      /*  private void OnDoneRewardAdsNextLevel()
        {
            RemoveRewardNextLevelEvent();
            NextLevel();
            SoundManager_BabyGirl.Instance.ReplaySoundBg();
        }*/

        public void OffHintButton()
        {
            ideaButton.gameObject.SetActive(false);
            replayButton.anchoredPosition = new Vector2(-150, replayButton.anchoredPosition.y);
            adsButton.anchoredPosition = new Vector2(150, adsButton.anchoredPosition.y);
        }
        public void OnHintButton()
        {
            ideaButton.gameObject.SetActive(true);
            replayButton.anchoredPosition = new Vector2(-231.85f, replayButton.anchoredPosition.y);
            adsButton.anchoredPosition = new Vector2(231.85f, adsButton.anchoredPosition.y);
        }

        public void ShowHint()
        {
            if (currentLevel.CanShowHint() && GetHintCount() > 0)
            {
                currentLevel.ShowHint();
                SetHintCount(GetHintCount() - 1);
                OffHintButton();
            }
            else
            {
                //BonBonAdvertising.RewardedAdCompleted += OnDoneRewardAdsHint;
                //BonBonAdvertising.RewardedAdSkipped += RemoveRewardHintEvent;
                //PopupWatchVideoAds.OnHidePopupVideoAd += RemoveRewardHintEvent;
                //PopupWatchVideoAds.instance.Show();
            }
            UpdateHintCountText();
        }

        private void RemoveRewardHintEvent()
        {
            //BonBonAdvertising.RewardedAdCompleted -= OnDoneRewardAdsHint;
            //BonBonAdvertising.RewardedAdSkipped -= RemoveRewardHintEvent;
            //PopupWatchVideoAds.OnHidePopupVideoAd -= RemoveRewardHintEvent;
        }
        private void RemoveRewardNextLevelEvent()
        {
            //BonBonAdvertising.RewardedAdCompleted -= OnDoneRewardAdsNextLevel;
            //BonBonAdvertising.RewardedAdSkipped -= RemoveRewardNextLevelEvent;
            //PopupWatchVideoAds.OnHidePopupVideoAd -= RemoveRewardNextLevelEvent;
        }

       /* private void OnDoneRewardAdsHint()
        {
            RemoveRewardHintEvent();
            SetHintCount(GetHintCount() + 1);
            UpdateHintCountText();
            SoundManager_BabyGirl.Instance.ReplaySoundBg();
        }*/

        private void UpdateHintCountText()
        {
           // hintCountText.text = GetHintCount().ToString();
        }

        public static int GetLevel()
        {
            return PlayerPrefs.GetInt("Toilet Rush Level", 0);
        }

        public static void SetLevel(int level)
        {
            if (level > GetUnlockedLevel())
            {
                PlayerPrefs.SetInt("Toilet Rush Unlocked Level", level);
            }
            PlayerPrefs.SetInt("Toilet Rush Level", level % 36);
        }

        public static int GetUnlockedLevel()
        {
            return PlayerPrefs.GetInt("Toilet Rush Unlocked Level", 0);
        }

        private void OnDestroy()
        {
            instance = null;
            //BonBonAdvertising.OnNoInternet -= NoInternet;
            //BonBonAdvertising.OnAdsNoReady -= OnAdsNoReady;
            GCAddressable();
        }

        private void OnAdsNoReady()
        {
            //PopupManager.Instance.ShowToat("No have Ads");
            RemoveRewardHintEvent();
            RemoveRewardNextLevelEvent();
        }

        private void NoInternet()
        {
           // PopupManager.Instance.ShowToat("No internet");
            RemoveRewardHintEvent();
            RemoveRewardNextLevelEvent();
        }

        public void GCAddressable()
        {
            onGCAddressable?.Invoke();
            onGCAddressable.RemoveAllListeners();

           /* foreach (var b in objectAddressableHanlers)
            {
                if (b.IsValid())
                {
                    Addressables.Release(b);
                }
            }

            foreach (var t in spriteAddressableHanlers)
            {
                if (t.IsValid())
                {
                    Addressables.Release(t);
                }
            }*/
        }


       // static AsyncOperationHandle<SceneInstance> sceneAddressableOperation;
        public static void LoadMainGame()
        {
            //Manager.instance.transition.FullScreenUntil(LoadSceneAsync());
            //IEnumerator LoadSceneAsync()
            //{
            //    GCScene();
            //    //sceneAddressableOperation = Addressables.LoadSceneAsync("ToiletRush");
            //    SceneManager.LoadScene("ToiletRush");
            //    yield return sceneAddressableOperation;
            //}

            SceneManager.LoadScene("ToiletRush");
        }

        public static void GCScene()
        {
           /* if (sceneAddressableOperation.IsValid())
                Addressables.Release(sceneAddressableOperation);*/
        }

        private int GetHintCount()
        {
            return PlayerPrefs.GetInt("ToiletRushHint", 3);
        }

        private void SetHintCount(int hint)
        {
            PlayerPrefs.SetInt("ToiletRushHint", hint);
        }
    }

    [System.Serializable]
    public struct StringArray
    {
        public string[] array;
    }

    public enum Gender
    {
        Boy = 0,
        Girl = 1,
        None = 2,
    }

}

