using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HongQuan
{
    [RequireComponent(typeof(Button))]   
    public class ButtonSound : MonoBehaviour
    {
        public AudioClip sound;
        private void Awake()
        {
            var but = GetComponent<Button>();
            but.onClick.AddListener(()=>SoundManager_BabyGirl.Instance.PlayOneShot(sound));
        }
    }

}
