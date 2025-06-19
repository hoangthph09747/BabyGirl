using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ExtendedAnimation
{
    public class UIAnimation : MonoBehaviour
    {
        [SerializeField] private UIShowType showType = UIShowType.Bottom;
        [SerializeField] private float duration = 0.7f;
        [SerializeField] private Ease ease = Ease.InOutBack;
        [SerializeField] private float delay;

        private RectTransform canvasRect;
        private RectTransform rectTransform;
        private bool canAnim = true;
        private Vector2 basePos;

        public UnityEvent onShow;
        public UnityEvent onHide;

        public UnityEvent onStartShow;
        public UnityEvent onStartHide;

        public float TimeAnim => delay + duration;
        public enum UIShowType
        {
            None,
            Bottom,
            Left,
            Right,
            Top,
        }
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            basePos = rectTransform.anchoredPosition;
            //Debug.Log(basePos);
            canvasRect = GetComponentInParent<Canvas>().rootCanvas.GetComponent<RectTransform>();
        }

        public void ResetBasePosition()
        {
            basePos = rectTransform.anchoredPosition;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            if (!canAnim) return;
            canAnim = false;
            onStartShow?.Invoke();
            Vector2 startPos = Vector2.zero;
            float multiX = (rectTransform.anchorMax.x - rectTransform.anchorMin.x) + 1f;
            float multiY = (rectTransform.anchorMax.y - rectTransform.anchorMin.y) + 1f;
            switch (showType)
            {
                case UIShowType.None:
                    canAnim = true;
                    return;
                case UIShowType.Bottom:
                    startPos = new Vector2(rectTransform.anchoredPosition.x, (canvasRect.rect.height + rectTransform.rect.height) / -2 * multiY);
                    break;
                case UIShowType.Left:
                    startPos = new Vector2((canvasRect.rect.width + rectTransform.rect.width) / -2 * multiX, rectTransform.anchoredPosition.y);
                    break;
                case UIShowType.Right:
                    startPos = new Vector2((canvasRect.rect.width + rectTransform.rect.width) / 2 * multiX, rectTransform.anchoredPosition.y);
                    break;
                case UIShowType.Top:
                    startPos = new Vector2(rectTransform.anchoredPosition.x, (canvasRect.rect.height + rectTransform.rect.height) / 2 * multiY);
                    break;
            }
            rectTransform.anchoredPosition = startPos;
            Debug.Log("Show "+ name);
            rectTransform.DOAnchorPos(basePos, duration).SetEase(ease).OnComplete(() => onShow?.Invoke()).SetDelay(delay);
            Invoke("CanAnim", duration + delay);
        }

        private void CanAnim()
        {
            canAnim = true;
        }

        public void Hide()
        {
            if (!canAnim) return;
            Vector2 hidePos = Vector2.zero;
            canAnim = false;
            onStartHide?.Invoke();
            float multiX = (rectTransform.anchorMax.x - rectTransform.anchorMin.x) + 1f;
            float multiY = (rectTransform.anchorMax.y - rectTransform.anchorMin.y) + 1f;
            switch (showType)
            {
                case UIShowType.None:
                    canAnim = true;
                    return;
                case UIShowType.Bottom:
                    hidePos = new Vector2(rectTransform.anchoredPosition.x, (canvasRect.rect.height + rectTransform.rect.height) / -2 * multiY);
                    break;
                case UIShowType.Left:
                    hidePos = new Vector2((canvasRect.rect.width + rectTransform.rect.width) / -2 * multiX, rectTransform.anchoredPosition.y);
                    break;
                case UIShowType.Right:
                    hidePos = new Vector2((canvasRect.rect.width + rectTransform.rect.width) / 2 * multiX, rectTransform.anchoredPosition.y);
                    break;
                case UIShowType.Top:
                    hidePos = new Vector2(rectTransform.anchoredPosition.x, (canvasRect.rect.height + rectTransform.rect.width) / 2 * multiY);
                    break;
            }
            rectTransform.DOAnchorPos(hidePos, duration).SetEase(ease).OnComplete(() => { onHide?.Invoke(); gameObject.SetActive(false); });
            Invoke("CanAnim", duration + delay);
        }

    }


#if UNITY_EDITOR
    [CanEditMultipleObjects, CustomEditor(typeof(UIAnimation))]
    public class UIAnimationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Show"))
                Show();
            if (GUILayout.Button("Hide"))
                Hide();
        }

        private void Show()
        {
            ((UIAnimation)target).Show();
        }

        private void Hide()
        {
            ((UIAnimation)target).Hide();
        }
    }

#endif
}

