using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToiletRush
{
    public class ToiletRushEndGameSetToilet : MonoBehaviour
    {
        [SerializeField] bool setGender = false;
        [SerializeField] Gender gender;
        
        private void Awake()
        {
            int gender = (int)this.gender;
            if (!setGender) gender = (int)ToiletRushManager.instance.LoseGender;
            Image image = GetComponent<Image>();
            image.sprite = ToiletRushManager.instance.currentToilet[gender];

            RectTransform parentRect = image.rectTransform.parent.GetComponent<RectTransform>();
            Vector2 nativeSize = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
            float widthRatio = parentRect.rect.width / nativeSize.x;
            float heightRatio = parentRect.rect.height / nativeSize.y;
            float scale = Mathf.Min(widthRatio, heightRatio);
            image.rectTransform.sizeDelta = new Vector2(nativeSize.x * scale, nativeSize.y * scale);
        }
    }

}