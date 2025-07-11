using UnityEngine;
using System.Collections;
using PaintCraft.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PaintCraft.Controllers;
using DG.Tweening;

namespace PatinCraft.UI
{
    public class ChangeColorOnClickController : MonoBehaviour
    {
        private Toggle toggle;
        public LineConfig LineConfig;
        public Color Color;


        void Start()
        {
            toggle = GetComponent<Toggle>();
            InitValidate();
            ToggleListener();
        }

        private void InitValidate()
        {
            if (LineConfig == null)
            {
                Debug.LogError("LineConfig must be provided", gameObject);
            }
        }

        public void ToggleListener()
        {
            toggle.onValueChanged.AddListener(OnValueChange);
            if (toggle.isOn) { OnValueChange(true); }
        }
        public void OnValueChange(bool arg0)
        {
            if (arg0)
            {
            
            }
        }
    }
}