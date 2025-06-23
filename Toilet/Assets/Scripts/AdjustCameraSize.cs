using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HongQuan
{
    [RequireComponent(typeof(Camera))]
    public class AdjustCameraSize : MonoBehaviour
    {
        public float minCameraWidth = 5f;
        public UnityEvent<float> onDone;

        void Awake()
        {
            AdjustCameraOrthographicSize();
        }

        void AdjustCameraOrthographicSize()
        {
            Camera cam = GetComponent<Camera>();

            float aspect = (float)Screen.width / Screen.height;
            float camWidth = cam.orthographicSize * aspect;

            //Debug.Log("Cam width = "+camWidth);
            if (camWidth >= minCameraWidth) return;
            cam.orthographicSize *= minCameraWidth / camWidth;
            onDone?.Invoke(cam.orthographicSize);
        }
    }
}