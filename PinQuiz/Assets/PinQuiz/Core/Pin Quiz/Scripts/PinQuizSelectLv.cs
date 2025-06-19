using HongQuan;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PinQuiz
{
    public class PinQuizSelectLv : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        [SerializeField] private Color lockColor;

        [SerializeField] Sprite lockSprite;

        public int level;
        private Color textColor;
        public bool isLock;

        public Text Text => text;

        public void Init(int level)
        {
            this.level = level;
            textColor = text.color;
            text.text = (level + 1).ToString();
            int l = PlayerPrefs.GetInt("Unlock Pin Quiz Level", 0);
            isLock = level > l;
            if (isLock)
            {
                image.sprite = lockSprite;
                text.color = lockColor;
            }
        }

        public void PlayGame()
        {
            if (isLock) return;
            PlayerPrefs.SetInt("Pin Quiz Level", level);
            //BonBonAdvertising.ShowInterstitialAd(LoadSceneManager.Instance.nameMinigame.ToString());
            Manager.instance.transition.FullScreen(() =>
            {
                //LoadSceneManager.Instance.LoadScene("MainPinQuiz");
                //try
                //{
                //    LoadSceneManager.Instance.LoadScene("MainPinQuiz");
                //}
                //catch
                //{
                //    SceneManager.LoadScene("MainPinQuiz");
                //}
            }, null);
        }

        public void Unlock()
        {
            isLock = false;
            image.color = Color.white;
            text.color = textColor;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PinQuizSelectLv), editorForChildClasses: true), CanEditMultipleObjects]
    public class PinQuizSelectLvEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update Paramaters"))
            {
                UpdateParameters();
            }
        }

        private void UpdateParameters()
        {
            Undo.RecordObjects(targets, "Set Paramaters Fishing Level");
            foreach (var tar in targets)
            {
                var obj = tar as PinQuizSelectLv;

                string a = obj.name;
                string b = string.Empty;
                int val = 0;

                for (int i = 0; i < a.Length; i++)
                {
                    if (Char.IsDigit(a[i]))
                        b += a[i];
                }

                if (b.Length > 0) val = int.Parse(b);

                EditorUtility.SetDirty(obj.Text);
                obj.level = val;
                obj.Text.text = val.ToString();

            }

            //(serializedObject.FindProperty("text").objectReferenceValue as Text).text = serializedObject.FindProperty("level").intValue.ToString();
            //serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}