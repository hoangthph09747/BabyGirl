using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HongQuan
{
    [RequireComponent(typeof(Button))]
    public class LogEventButton : LogEvent
    {
        public string message;
        private void Awake()
        {
            var but = GetComponent<Button>();
            but.onClick.AddListener(()=>Log(message));
        }
    }
}