using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizMenu : MonoBehaviour
    {
        public static PinQuizMenu instance;

        public List<PinQuizSelectLv> selectLvs = new List<PinQuizSelectLv>();

        public PinQuizSelectLv buttonPrefab;
        public Transform content;

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Start()
        {
            for (int i = 0; i < 60; i++)
            {
                var b = Instantiate(buttonPrefab, content);
                b.Init(i);
                selectLvs.Add(b);
            }
            buttonPrefab.gameObject.SetActive(false);
            //UnlockAll();
            //MyAdsBabyGirl.GetInstance().HideBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
            
            //MyAdsBabyGirl.GetInstance().ShowBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
        }

        public void UnlockAll()
        {
            foreach (var lv in selectLvs)
            {
                lv.Unlock();
            }
        }
    }
}
