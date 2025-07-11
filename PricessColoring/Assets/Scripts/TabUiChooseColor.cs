using Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabUiChooseColor : MonoBehaviour
{
    public TypePen typePen;
    [SerializeField]
    GameObject effectSelect;
    [SerializeField]
    bool selected = false;
    Button button;
    [SerializeField]
    Image bg, icon;
    Action<object> _OnClickButtonTabColor;

    public float valueScrollBegin, valueEndScroll;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Click);
        _OnClickButtonTabColor = (param) => OnClickButtonTabColor((TabUiChooseColor)param);
        this.RegisterListener(EventID.OnClickButtonTabDecordMakeup, _OnClickButtonTabColor);
        
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnClickButtonTabDecordMakeup, _OnClickButtonTabColor);
    }

    private void OnClickButtonTabColor(TabUiChooseColor param)
    {
        Select(param == this);
    }

    void Click()
    {
        selected = true;
       // Select(selected);
        this.PostEvent(EventID.OnClickButtonTabColor, this);
        SoundManager_BabyGirl.Instance.PlayOneShot("Sounds/UI/Button");
       //BonBonAnalytics.GetInstance().LogEvent("btn_tab_color" + "_" + typePen.ToString());
    }

    private void Select(bool isSelect)
    {
        selected = isSelect;
        effectSelect.SetActive(selected);

        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
        //{
        //    if (selected == true)
        //    {
        //        if (gameObject.name == "ButtonSticker" || gameObject.name == "ButtonMocKhoa" ||
        //            gameObject.name == "ButtonDayTui" ||
        //            gameObject.name == "ButtonPhuKien")
        //        {
        //            DIY.GameManager_DIY.Instance.drawing = false;
        //        }
        //        else if (gameObject.name == "ButtonColor" || gameObject.name == "ButtonGlitter" ||
        //            gameObject.name == "ButtonOmbre" ||
        //            gameObject.name == "ButtonPattern")
        //        {
        //            DIY.GameManager_DIY.Instance.drawing = true;
        //        }
        //    }
        //}
    }

    [SerializeField]
    float scrollValueNow = 0;
    public void CheckSelectWithScrollValue(float value, bool isLeft, bool isRight)
    {

        scrollValueNow = value;
        if (isLeft)
        {
            Select((scrollValueNow >= valueScrollBegin && scrollValueNow <= valueEndScroll) ||
                scrollValueNow < valueScrollBegin);
        }
        else if (isRight)
        {
            Select((scrollValueNow >= valueScrollBegin && scrollValueNow <= valueEndScroll) || scrollValueNow > valueEndScroll);
        }
        else
        {
            Select(scrollValueNow >= valueScrollBegin && scrollValueNow <= valueEndScroll);
        }
    }

    public float GetValueScrollCenter()
    {
        return valueScrollBegin + (valueEndScroll - valueScrollBegin) * 0.15f;
    }
}
