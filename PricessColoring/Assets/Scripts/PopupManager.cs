using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PopupManager : MonoBehaviour
{
    public static PopupManager _instance;
    [SerializeField]
    Transform toat;
    [SerializeField]
    Text textToat;
    [SerializeField]
    Transform popupCheckInternet;
    public static PopupManager Instance
    {
        get
        {
            // Nếu chưa có thể hiện, hãy tạo một thể hiện mới
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();

                // Nếu không tìm thấy trong scene, tạo một GameObject mới chứa thể hiện
                if (_instance == null)
                {
                    GameObject singletonObject = Instantiate(Resources.Load<GameObject>("Prefabs/CanvasPopup"));
                    _instance = singletonObject.GetComponent<PopupManager>();
                }
            }

            // Trả về thể hiện duy nhất của class
            return _instance;
        }
    }

    private void Awake()
    {
        // Đảm bảo rằng chỉ có một thể hiện duy nhất tồn tại
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có một thể hiện duy nhất tồn tại, hủy bỏ thể hiện hiện tại
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowToat(string content)
    {
        DOTween.Kill(toat);
        textToat.text = content;
        toat.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() => {
            toat.DOScale(Vector3.zero, 0.25f).SetDelay(1);
        });
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Success");
    }   
    
    public void ShowCheckInternet()
    {
        popupCheckInternet.gameObject.SetActive(true);
    }    

    public void HideCheckInternet()
    {
        popupCheckInternet.gameObject.SetActive(false);
    }    
}
