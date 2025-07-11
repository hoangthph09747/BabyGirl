using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TogglePenSize : MonoBehaviour, IPointerClickHandler
{
    private Toggle togglePenSize;

    [SerializeField] private float size;

    // Start is called before the first frame update
    void Start()
    {
        togglePenSize = GetComponent<Toggle>();
        togglePenSize.onValueChanged.AddListener(OnValueChange);
        if (togglePenSize.isOn)
            OnValueChange(true);
    }

    private void OnValueChange(bool isOn)
    {
        if (isOn)
        {
            //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
            //{
            //    foreach (var lineConfig in DrawPictureController.Instance.ListLineConfig)
            //    {
            //        lineConfig.Scale = size;
            //    }
            //}
            //else
            //{
            //    foreach (var lineConfig in DrawPictureControllerASMR.Instance.ListLineConfig)
            //    {
            //        lineConfig.Scale = size;
            //    }
            //}

            foreach (var lineConfig in DrawPictureController.Instance.ListLineConfig)
            {
                lineConfig.Scale = size;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // SoundManager.Instance?.PlayUISound(SoundType.Button);
    }
}