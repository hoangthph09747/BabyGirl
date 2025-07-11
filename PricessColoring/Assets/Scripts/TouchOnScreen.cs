using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOnScreen : MonoBehaviour
{
    public static TouchOnScreen instance;
    [SerializeField]
    GameObject effect, effectFollow;
    [SerializeField]
    GameObject[] effectPrefabs;
    [SerializeField]
    bool addMultyEffect = false;
    [SerializeField]
    Camera camera;
    [SerializeField]
    bool playSoundTouch = false;
    public bool showEffectFollow = false;
    [SerializeField]
    AudioClip[] soundTouch;

    [SerializeField]
    bool ignoreOnUI = false;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!camera)
        camera = Camera.main;
    }

    void OnDestroy()
    {
        instance = null;
    }

    Vector3 mousePosition;
    // Update is called once per frame
    void Update()
    {
        if(ignoreOnUI)
        {
            if (GameManager.GetInstance().IsPointerOverUIObject())
                return;
        }

        if (Input.GetMouseButtonDown(0)) // Kiểm tra chuột đã được nhấn chưa (0 là chuột trái)
        {
            mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            ShowEffect(mousePosition);
        }
        else if(Input.GetMouseButton(0))
        {
            if (effectFollow && showEffectFollow)
            {
                mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                effectFollow.SetActive(true);
                effectFollow.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
        }    
        else if(Input.GetMouseButtonUp(0))
        {
            if (effectFollow && showEffectFollow)
                effectFollow.SetActive(false);
        }    

        
    }

    void ShowEffect(Vector3 p)
    {
        if (playSoundTouch)
        {
            if(soundTouch.Length <= 0)
            SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Bucket");
            else
            {
                SoundManager_BabyGirl.Instance.PlayOneShot(soundTouch[UnityEngine.Random.Range(0, soundTouch.Length)]);
            }    
        }

        if (effectPrefabs.Length <= 0)
        {
            if (effect)
            {
                effect.SetActive(false);
                effect.transform.position = new Vector3(p.x, p.y, 0);
                effect.SetActive(true);
            }
        }
        else
        {
            GameObject e = Instantiate(effectPrefabs[UnityEngine.Random.Range(0, effectPrefabs.Length)]);
            e.transform.position = new Vector3(p.x, p.y, 0);
            e.SetActive(true);
            Destroy(e, 4);
        }    
    }    
}
