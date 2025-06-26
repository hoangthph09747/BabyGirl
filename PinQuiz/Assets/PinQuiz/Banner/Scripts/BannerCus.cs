
using UnityEngine;

public class BannerCus : MonoBehaviour
{
    [SerializeField] GameObject BannerNgang;
    [SerializeField] GameObject BannerDoc;
    [SerializeField] GameObject PannerOver;
    void Start()
    {
        
        if ((float)Screen.width > (float)Screen.height)
        {
            BannerNgang.SetActive(true);   // Banner Landscape
            PannerOver.transform.localScale = new Vector3(1.3f,1.3f,1);
        }
        else
        {
            BannerDoc.SetActive(true);   // Banner Portrait
        }

    }

}
