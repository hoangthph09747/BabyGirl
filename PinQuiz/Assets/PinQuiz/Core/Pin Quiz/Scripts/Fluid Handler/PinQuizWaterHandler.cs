using PinQuiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PinQuiz
{
    public class PinQuizWaterHandler : MonoBehaviour
    {
        private void Start()
        {
            PinQuizManager.instance.SetActiveWaterCamera(true);
        }
    }
}

