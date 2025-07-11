using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PrincessColoring;
public class PopupViewPicturePrincessColoring : MonoBehaviour
{
    public static PopupViewPicturePrincessColoring instance;
    public string pathScreenshot, pathThumb;
    public Image bg;
    public RawImage screenshot;
    public Button buttonDelete, buttonSave, buttonBack;
    BasicGridAdapterGalleryPrincessColoring galleryPrincessColoring;
    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        buttonBack.onClick.AddListener(ClickBack);
        buttonDelete.onClick.AddListener(ClickDelete);
        buttonSave.onClick.AddListener(ClickSave);
    }

    public void Show(string pathScreenshot, string pathThumb)
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Star_Collect");
        this.pathScreenshot = pathScreenshot;
        this.pathThumb = pathThumb;
        if (screenshot.texture != null)
            Destroy(screenshot.texture);
        screenshot.texture = null;
        // TODO: Load texture from Resources or another method, since SaveSystem is removed.
        // screenshot.texture = SaveSystem.LoadTexture2D(pathScreenshot);
        screenshot.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack);
        bg.DOFade(0.85f, 0.5f);
        bg.raycastTarget = true;
        
        buttonBack.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(1);
        buttonDelete.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(1);
        buttonSave.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).SetDelay(1);

        if (!galleryPrincessColoring)
            galleryPrincessColoring = FindObjectOfType<BasicGridAdapterGalleryPrincessColoring>();
       
    }

   public  void Hide()
    {
        bg.DOFade(0, 0.5f);
        bg.raycastTarget = false;
        DOTween.Kill(screenshot.transform);
        DOTween.Kill(buttonBack.transform);
        DOTween.Kill(buttonDelete.transform);
        DOTween.Kill(buttonSave.transform);
        screenshot.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.OutCubic);
        buttonBack.transform.DOScale(Vector3.zero, 0.0f).SetEase(Ease.OutCubic);
        buttonDelete.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic).SetDelay(0);
        buttonSave.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutCubic);
    }

    void ClickBack()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        Hide();
    }

    void ClickDelete()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Delete");
        galleryPrincessColoring.gameObject.SetActive(false);
        // SaveSystem.DeleteFile(pathScreenshot);
        // SaveSystem.DeleteFile(pathThumb);
        // TODO: Delete files using System.IO.File.Delete or another method, since SaveSystem is removed.
        Hide();
        galleryPrincessColoring.gameObject.SetActive(true);
    }

    void ClickSave()
    {
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
        buttonSave.transform.DOScale(Vector3.zero, 0.15f);
      //  SaveToGallery((Texture2D)screenshot.texture);
    }    

   
}
