using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToiletRush
{
    public class ToiletRushLevel : MonoBehaviour
    {
        [SerializeField] bool isShowHint = false;

        ToiletRushCharacter[] characters;

        public UnityEvent onCharactersMove;

        private void Start()
        {
            characters = GetComponentsInChildren<ToiletRushCharacter>();
            ToiletRushDrawLine.instance.onDrawSuccess.AddListener(MoveCharacters);
            foreach (var character in characters)
            {
                character.SetLevel(this);
                character.Toilet.onGoToToilet.AddListener(OnCharacterGoToToilet);
            }
            ToiletRushDrawLine.instance.EnableDraw();

            if (isShowHint)
            {
                ShowHint();
            }
        }

        int characterDoneDrawCount = 0;
        private void MoveCharacters(ToiletRushDrawLine drawline)
        {
            characterDoneDrawCount++;

            //Turn next hint
            if (isShowHint)
                ShowHint();

            //Move if all draw Done
            if (characterDoneDrawCount == characters.Length)
            {
                foreach (var character in characters)
                {
                    character.Move();
                }
                onCharactersMove?.Invoke();
                OffHint();
                ToiletRushDrawLine.instance.DisableDraw();
            }
        }

        int characterReadyGoToToiletCount = 0;
        public void OnCharacterGoToToilet(ToiletRushToilet toilet)
        {
            toilet.onGoToToilet.RemoveListener(OnCharacterGoToToilet);
            characterReadyGoToToiletCount++;
            if (characterReadyGoToToiletCount == characters.Length)
            {
                ToiletRushManager.instance.WinGame();
            }
        }

        public void ShowHint()
        {
            isShowHint = true;

            bool canHint = false;
            foreach (var character in characters)
            {
                if (!character.HavePath && !canHint)
                {
                    canHint = true;
                    character.GuideLine.gameObject.SetActive(true);
                }
                else
                {
                    character.GuideLine.gameObject.SetActive(false);
                }
            }
        }

        public bool CanShowHint()
        {
            foreach (var character in characters)
                if (!character.HavePath)
                    return true;

            return false;
        }

        public void OffHint()
        {
            isShowHint = false;
            foreach (var character in characters)
            {
                character.GuideLine.gameObject.SetActive(false);
            }
        }
    }

}