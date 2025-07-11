using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Observer;
public class UIChooseColorManager : MonoBehaviour
{
    [SerializeField]
    Transform contentTab, contentGroup;
    [SerializeField]
    public ScrollRect scrollListColor, scrollTabColor;
    [SerializeField]
    public List<TabUiChooseColor> listTabUiChooseColor;
    Action<object> _OnClickButtonTabColor;
    Action<object> _OnClickItemUiColor;
    // Start is called before the first frame update
    void Start()
    {
        scrollListColor.onValueChanged.AddListener(OnScrollValueChanged);
        
        Invoke(nameof(SetValueScrollStartAndEndOfButtonTabDecord), 0.15f);

        _OnClickButtonTabColor = (param) => OnClickButtonTabColor((TabUiChooseColor)param);
        this.RegisterListener(EventID.OnClickButtonTabColor, _OnClickButtonTabColor);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnClickButtonTabColor, _OnClickButtonTabColor);
    }

    private void OnClickButtonTabColor(TabUiChooseColor param)
    {
       // scrollListColor.DOHorizontalNormalizedPos(param.GetValueScrollCenter(), 0.25f).SetEase(Ease.Linear);
       // ScrollToSelectedItem_Tab(scrollListColor, param.GetComponent<RectTransform>());

        ScrollTo(scrollListColor, param.GetValueScrollCenter() , 1);
    }

    [SerializeField]
    bool checkScrollAble = false;
    private void OnScrollValueChanged(Vector2 scrollValue)
    {
        //Debug.LogError("Scrolling: " + scrollValue);
        scrollTabColor.horizontalNormalizedPosition = scrollValue.x;
        for (int i = 0; i < listTabUiChooseColor.Count; i++)
        {
            if (i == 0)
                listTabUiChooseColor[i].CheckSelectWithScrollValue(scrollValue.x, true, false);
            else if (i == listTabUiChooseColor.Count - 1)
                listTabUiChooseColor[i].CheckSelectWithScrollValue(scrollValue.x, false, true);
            else
                listTabUiChooseColor[i].CheckSelectWithScrollValue(scrollValue.x, false, false);
        }
    }

    public void ScrollTo(ScrollRect scrollRect, float value, float time)
    {
        //scrollRect.horizontalNormalizedPosition = value;
        scrollRect.DOHorizontalNormalizedPos(value, time).SetEase(Ease.Linear);
    }

    void SetValueScrollStartAndEndOfButtonTabDecord()
    {
        float lenghtTotal = contentGroup.GetComponent<RectTransform>().sizeDelta.x;
        float paddingLeft = contentGroup.GetComponent<HorizontalLayoutGroup>().padding.left;
        float paddingRight = contentGroup.GetComponent<HorizontalLayoutGroup>().padding.right;
        float spacing = contentGroup.GetComponent<HorizontalLayoutGroup>().spacing;
        for (int i = 0; i < listTabUiChooseColor.Count; i++)
        {
            if (i == 0)
            {
                listTabUiChooseColor[i].valueScrollBegin = 0;
                listTabUiChooseColor[i].valueEndScroll =
                        (paddingLeft + contentGroup.GetChild(i).GetComponent<RectTransform>().sizeDelta.x) * 1.0f / lenghtTotal;
            }
            else
            {
                listTabUiChooseColor[i].valueScrollBegin = listTabUiChooseColor[i - 1].valueEndScroll;
                float lenght = 0;
                lenght += paddingLeft;
                for (int j = 0; j <= i; j++)
                {
                    lenght += contentGroup.GetChild(j).GetComponent<RectTransform>().sizeDelta.x;
                    if (j > 0)
                    {
                        lenght += spacing;
                    }
                }
                listTabUiChooseColor[i].valueEndScroll = lenght * 1.0f / lenghtTotal;
            }
        }

        ScrollTo(scrollListColor, listTabUiChooseColor[0].valueScrollBegin , 1);
    }

    public void ScrollToSelectedItem_Tab(ScrollRect scrollRect, RectTransform selectedItem)
    {
        // Lấy kích thước của viewport
        Vector2 viewportSize = new Vector2(scrollRect.viewport.rect.width, scrollRect.viewport.rect.height);

        // Lấy kích thước của item được chọn
        Vector2 itemSize = selectedItem.rect.size;

        // Tính toán vị trí cuộn để đưa item chọn vào giữa màn hình ngang
        float xOffset = (selectedItem.localPosition.x + itemSize.x * 0.5f) - (viewportSize.x * 0.5f);

        // Giới hạn cuộn vào khoảng [0, 1]
        float normalizedXOffset = Mathf.Clamp01(xOffset / (scrollRect.content.sizeDelta.x - viewportSize.x));

        // Thiết lập vị trí cuộn ngang
        scrollRect.horizontalNormalizedPosition = normalizedXOffset;
        //ScrollTo(scrollRect, normalizedXOffset, 0.25f);
        //scrollRect.DOHorizontalNormalizedPos(normalizedXOffset, 0.15f).SetEase(Ease.Linear).SetDelay(0.5f);
    }

    public void ScrollToSelectedItem_LisContent(ScrollRect scrollRect, RectTransform selectedItem)
    {
        // Lấy kích thước của viewport
        Vector2 viewportSize = new Vector2(scrollRect.viewport.rect.width, scrollRect.viewport.rect.height);

        // Lấy kích thước của item được chọn trong không gian của ScrollRect
        Vector2 itemSize = selectedItem.rect.size;

        // Lấy vị trí của item được chọn trong không gian của ScrollRect
        Vector3 itemPositionInScrollRect = selectedItem.localPosition + selectedItem.parent.localPosition;

        // Tính toán vị trí cuộn để đưa item chọn vào giữa màn hình ngang
        float xOffset = (itemPositionInScrollRect.x + itemSize.x * 0.5f) - (viewportSize.x * 0.5f);

        // Giới hạn cuộn vào khoảng [0, 1]
        float normalizedXOffset = Mathf.Clamp01(xOffset / (scrollRect.content.sizeDelta.x - viewportSize.x));

        // Thiết lập vị trí cuộn ngang
        //scrollRect.horizontalNormalizedPosition = normalizedXOffset;
        ScrollTo(scrollRect, normalizedXOffset, 0.25f);
    }
}
