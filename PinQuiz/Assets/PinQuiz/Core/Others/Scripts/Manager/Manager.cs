using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuanUtilities;

namespace HongQuan
{
    public class Manager : MonoBehaviour
    {
        private static Manager _instance;
        public static Manager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<Manager>("Manager"));
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public Transition transition;
       // public AddressableHelper addressableHelper;
        public GameSoundManager soundManager;
    }
}


