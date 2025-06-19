using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ExtendedAnimation
{
    public class UIAnimationParent : MonoBehaviour
    {
        public UIAnimation[] animArray;
        public bool isAutoGetAnim;
        public bool playOnEnable;

        public UnityEvent onShow;
        public UnityEvent onHide;

        private void Awake()
        {
            if (isAutoGetAnim)
                animArray = GetComponentsInChildren<UIAnimation>();
        }

        private void OnEnable()
        {
            if (playOnEnable)
                Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < animArray.Length; i++)
                animArray[i].Show();
            onShow?.Invoke();
        }
        public void Hide()
        {
            float latestTime = 0f;
            for (int i = 0; i < animArray.Length; i++)
            {
                animArray[i].Hide();
                if (latestTime < animArray[i].TimeAnim)
                    latestTime = animArray[i].TimeAnim;
            }
            Invoke("HideItseft", latestTime);
            onHide?.Invoke();
        }
        private void HideItseft()
        {
            gameObject.SetActive(false);
        }
    }
}


