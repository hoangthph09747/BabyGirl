using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuHomePrincessColoring : MonoBehaviour
{
    [SerializeField]
    Button buttonExit, buttonGallery;
    [SerializeField]
    GameObject galleryUI, selectPictureUI;
    // Start is called before the first frame update
    void Start()
    {
        //BonBonAnalytics.GetInstance().LogEvent("game_" + LoadSceneManager.Instance.nameMinigame.ToString() + "_start");

        buttonExit.onClick.AddListener(ClickExit);
        buttonGallery.onClick.AddListener(ShowGallery);
        SoundManager_BabyGirl.Instance.PlayBgSound("Sounds/Backgrounds/BG_Coloring");
        //AddressableManager.Instance.StartLoadPageConfig();
        Invoke(nameof(ShowPicturesUI), 0.5f);

        //MyAdsBabyGirl.GetInstance().HideBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
        //MyAdsBabyGirl.GetInstance().ShowBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());

        //AddressableManager.Instance.isLoadingSinglePage = false;
    }

    void ShowPicturesUI()
    {
        selectPictureUI.SetActive(true);
    }    

    private void OnDestroy()
    {
        //MyAdsBabyGirl.GetInstance().HideBannerAd(LoadSceneManager.Instance.nameMinigame.ToString());
    }

    void ClickExit()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        //LoadSceneManager.Instance.LoadScene(Constant.SceneMenu);
        SceneManager.LoadScene(Constant.SceneMenu);
    }

    void ShowGallery()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        galleryUI.SetActive(true);
    }

    public void HideGallery()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        galleryUI.SetActive(false);
    }    
}
