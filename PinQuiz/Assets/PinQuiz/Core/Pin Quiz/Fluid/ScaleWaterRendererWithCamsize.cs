using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class ScaleWaterRendererWithCamsize : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private bool autoScale;

        private void Start()
        {
            if (autoScale) ScaleWaterRenderer();
        }

        public void ScaleWaterRenderer()
        {
            float size = cam.orthographicSize*2;
            transform.localScale = Vector3.one * size;
        }
    }

}
