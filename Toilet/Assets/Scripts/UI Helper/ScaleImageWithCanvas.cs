using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HongQuan
{
    [RequireComponent(typeof(Image))]
    public class ScaleImageWithCanvas : MonoBehaviour
    {
        [SerializeField] RectTransform canvas;

        private void Start()
        {
            if (canvas == null)
                canvas = (RectTransform)transform.root;

            Image img = GetComponent<Image>();

            img.rectTransform.anchoredPosition = Vector2.zero;
            img.SetNativeSize();

            float multiplier,t;
            Debug.Log(canvas.sizeDelta);
            multiplier = canvas.sizeDelta.x / img.rectTransform.sizeDelta.x;
            t = canvas.sizeDelta.y / img.rectTransform.sizeDelta.y;
            if(multiplier < t) multiplier = t;
            img.rectTransform.sizeDelta *= multiplier;
        }
    }

}