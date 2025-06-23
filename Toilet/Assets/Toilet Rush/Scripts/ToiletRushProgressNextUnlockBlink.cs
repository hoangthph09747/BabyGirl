using System.Collections;
using System.Collections.Generic;
using ToiletRush;
using UnityEngine;
using UnityEngine.UI;

public class ToiletRushProgressNextUnlockBlink : MonoBehaviour
{
    [SerializeField] float rate = 1f;
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(ChangeSprite), 0, rate);
    }

    int index = -1;
    private void ChangeSprite()
    {
        index = (index + 1) % ToiletRushManager.instance.nextToilet.Length;
        if(ToiletRushManager.instance.nextToilet[index] == null)
        {
            CancelInvoke();
            gameObject.SetActive(false);
            return;
        }
        image.sprite = ToiletRushManager.instance.nextToilet[index];
        AdjustImageSize();
    }

    private void AdjustImageSize()
    {
        RectTransform parentRect = image.rectTransform.parent as RectTransform;
        Vector2 nativeSize = new Vector2(image.sprite.rect.width, image.sprite.rect.height);
        float widthRatio = parentRect.rect.width / nativeSize.x;
        float heightRatio = parentRect.rect.height / nativeSize.y;

        float scale = Mathf.Min(widthRatio, heightRatio);

        image.rectTransform.sizeDelta = new Vector2(nativeSize.x * scale, nativeSize.y * scale);
    }
}
