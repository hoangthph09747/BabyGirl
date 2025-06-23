using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HongQuan
{
    [RequireComponent(typeof(RawImage))]
    public class MoveRawImage : MonoBehaviour
    {
        [SerializeField] private float speed = 0.1f;
        [SerializeField] private Vector2 direction = new Vector2(-1,-1);
        RawImage rawImage;
        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            direction = direction.normalized;
        }

        private void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + direction * speed * Time.deltaTime,rawImage.uvRect.size);
        }

    }
}
