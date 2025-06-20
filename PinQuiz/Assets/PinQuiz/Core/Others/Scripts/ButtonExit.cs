using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HongQuan
{
    //[RequireComponent(typeof(Button)), RequireComponent(typeof(ExtendedAnimation.UIAnimationButton))]
    public class ButtonExit : MonoBehaviour
    {
        public UnityEvent onStay;
        private void Awake()
        {
            var but = GetComponent<Button>();
            but.onClick.AddListener(TransitionToMenu);
        }

        public void TransitionToMenu()
        {
            //Manager.instance.transition.FullScreen
            //    (
            //    onStay: () =>
            //    {
            //        //SceneManager.LoadScene("Menu");
            //        onStay?.Invoke();
            //        LoadSceneManager.Instance.LoadScene(Constant.SceneMenu);
            //    },
            //    onOut: null
            //    );

            //LoadSceneManager.Instance.LoadScene(Constant.SceneMenu);
        }
    }
}

