using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HongQuan
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundPath : MonoBehaviour
    {
        public string path = "Sounds/UI/Button";
        private void Awake()
        {
            var but = GetComponent<Button>();
            but.onClick.AddListener(() =>
            SoundManager_BabyGirl.Instance.PlayOneShot(path));
        }
    }

}