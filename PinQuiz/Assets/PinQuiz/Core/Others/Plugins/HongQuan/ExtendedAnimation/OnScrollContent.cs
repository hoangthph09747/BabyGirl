using DG.Tweening;
using UnityEngine;

public class OnScrollContent : MonoBehaviour
{
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private float scrollDelta = -300;
    [SerializeField] private ScrollType scrollType;
    [SerializeField] private bool isLoop = true;
    [SerializeField] private bool isPlayOnEnabe = true;

    private RectTransform rectTransform;


    public enum ScrollType
    {
        Horizontal,
        Vertical
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (isPlayOnEnabe)
            ScrollContent();
    }

    public void ScrollContent()
    {
        switch (scrollType)
        {
            case ScrollType.Horizontal:
                rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + scrollDelta, duration).SetLoops(isLoop ? 2 : 0, LoopType.Yoyo).SetEase(Ease.InOutQuad);
                break;
            case ScrollType.Vertical:
                rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + scrollDelta, duration).SetLoops(isLoop ? 2 : 0, LoopType.Yoyo).SetEase(Ease.InOutQuad);
                break;
        }
    }
}
