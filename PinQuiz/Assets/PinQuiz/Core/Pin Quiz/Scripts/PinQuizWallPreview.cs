using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz {
    public class PinQuizWallPreview : MonoBehaviour
    {
        private void Start()
        {
            var mat = GetComponent<Renderer>().material;
            var color = mat.color;
            color.a = 0;
            mat.color = color;
        }
    }
}