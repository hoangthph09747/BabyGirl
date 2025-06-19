using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizLavaHandler : MonoBehaviour
    {
        private void Start()
        {
            PinQuizManager.instance.SetActiveLavaCamera(true);
        }
    }
}

