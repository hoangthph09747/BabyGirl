//using DG.Tweening;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AddressableAssets;

namespace ToiletRush
{
    public class ToiletRushLoseUI : MonoBehaviour
    {
        [SerializeField] Transform board;
        [SerializeField] float showDuration = 1;
        [SerializeField] AudioClip loseSound;

        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
           // MyAnalytics.GetInstance().LogLoseGame();
        }

        public void ShowHit(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Hit_Map.prefab");
        }

        public void ShowBanana(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Banana.prefab");
        }
        public void ShowGum(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Gum.prefab");
        }
        public void ShowBeetle(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Beetle.prefab");
        }
        public void ShowCrab(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Crab.prefab");
        }

        public void ShowSkateBoard(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Skate_Board.prefab");
        }
        public void ShowSnowSkateBoard(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Snow_Skate_Board.prefab");
        }
        public void ShowCat(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Cat_{ToiletRushManager.instance.Map}_{GetGender()}.prefab");
        }
        public void ShowPony(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Pony.prefab");
        }
        public void ShowSpider(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Spider.prefab");
        }
        public void ShowTomb(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Tomb.prefab");
        }
        public void ShowGhost(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Ghost.prefab");
        }
        public void ShowZombie(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Zombie.prefab");
        }
        public void ShowReindeer(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Reindeer.prefab");
        }
        public void ShowSnowman(float delay = 3f)
        {
            Show(delay, $"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/Lose/Lose_Snowman.prefab");
        }
        public string GetMapTwoMapString()
        {
            int number = ToiletRushManager.instance.Map;
            int baseNumber = (number / 2) * 2;
            return $"{baseNumber}{baseNumber + 1}";
        }

        public int GetGender()
        {
            return (int)ToiletRushManager.instance.LoseGender;
        }

        public void Show(float delay, string addrress)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0;
          //  StartCoroutine(StartShow(delay, addrress));
        }

       /* IEnumerator StartShow(float delay, string address)
        {
            float timeBeforeLoad = Time.time;

            var winObjectHandler = Addressables.LoadAssetAsync<GameObject>(address);
            ToiletRushManager.instance.objectAddressableHanlers.Add(winObjectHandler);
            yield return winObjectHandler;
            var endGameUI = Instantiate(winObjectHandler.Result);
            endGameUI.SetActive(false);
            endGameUI.transform.SetParent(board, true);
            (endGameUI.transform as RectTransform).anchoredPosition3D = Vector3.zero;
            (endGameUI.transform as RectTransform).localScale = Vector3.one;

            delay -= Time.time - timeBeforeLoad;
            if (delay > 0) yield return new WaitForSeconds(delay);

            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(loseSound);
            endGameUI.GetComponent<ToiletRushEndGameAnimUI>().Show(showDuration);
            canvasGroup.interactable = false;
            canvasGroup.DOFade(1, showDuration).OnComplete(() => canvasGroup.interactable = true);
        }*/
    }
}