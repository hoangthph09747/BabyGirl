using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class LogEvent : MonoBehaviour
    {
        public void Log(string message)
        {
            //BonBonAnalytics.GetInstance().LogEvent("btn_" + LoadSceneManager.Instance.nameMinigame.ToString() + "_" + message);
        }
    }
}