//using DG.Tweening;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ToiletRush
{
    public class ToiletRushWinUI : MonoBehaviour
    {
        [SerializeField] Transform winBoard;
        [SerializeField] float showDuration = 1;
        [SerializeField] Image progressBar;
        [SerializeField] GameObject newToiletText;
        [SerializeField] GameObject newToiletImg;
        [SerializeField] GameObject winText;
        [SerializeField] AudioClip winSound;
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(float delay = 3f)
        {
            SoundManager_BabyGirl.Instance.PlayOneShotLong(winSound);
           /* gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            if (ToiletRushManager.instance.Map < 11)
            {
                progressBar.rectTransform.parent.gameObject.SetActive(true);
                progressBar.fillAmount = (ToiletRushManager.instance.Level % 3) * 1.0f / 3;

                if (ToiletRushManager.instance.Level % 3 == 2)
                {
                    newToiletImg.SetActive(true);
                    newToiletText.SetActive(true);
                    winText.SetActive(false);
                }
            }
            StartCoroutine(StartShow(delay));*/
        }
       /* IEnumerator StartShow(float delay)
        {
            float timeBeforeLoad = Time.time;
            GameObject winObj = null;
            if (winText.activeSelf)
            {
                var winObjectHandler = Addressables.LoadAssetAsync<GameObject>($"Assets/HongQuan/Projects/Toilet Rush/Prefabs/UI/WIn/WinMap_{ToiletRushManager.instance.Map}_{Random.Range(0, 3)}.prefab");
                ToiletRushManager.instance.objectAddressableHanlers.Add(winObjectHandler);
                yield return winObjectHandler;
                winObj = Instantiate(winObjectHandler.Result);
                winObj.SetActive(false);
                winObj.transform.SetParent(winBoard, true);
                (winObj.transform as RectTransform).anchoredPosition3D = Vector3.zero;
                (winObj.transform as RectTransform).localScale = Vector3.one;
            }

            delay -= Time.time - timeBeforeLoad;
            if (delay > 0) yield return new WaitForSeconds(delay);

            if (winObj) winObj.GetComponent<ToiletRushEndGameAnimUI>().Show(showDuration);

            canvasGroup.interactable = false;
            canvasGroup.DOFade(1, showDuration).OnComplete(() => canvasGroup.interactable = true);
            progressBar.DOFillAmount(progressBar.fillAmount + 1f / 3, showDuration).SetDelay(showDuration / 2f);
            //SoundManager_BabyGirl.Instance.PlayOneShot("QuanSounds/Children Yay");
            SoundManager_BabyGirl.Instance.PlayOneShotLong(winSound);
        }*/

        [System.Serializable]
        public struct WinAnimAddress
        {
            public string[] addresses;
        }
    }

}