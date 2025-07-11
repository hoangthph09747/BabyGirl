using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelToolsView : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup groupTools;
    [SerializeField] private List<RectTransform> listToolDrawFull;
    [SerializeField] private List<RectTransform> listToolDrawLine;

    // Start is called before the first frame update
    void Start()
    {
        // Invoke("OffHorizontalLayoutGroup", 0.5f);
        StartCoroutine(OffHorizontalLayoutGroup());
    }
    
    

    IEnumerator OffHorizontalLayoutGroup()
    {
        yield return new WaitForSeconds(0.5f);
        listToolDrawFull.ForEach(o =>
        {
            o.GetComponent<ToggleTools>().posInit = o.anchoredPosition;
            o.gameObject.SetActive(false);
        });
        listToolDrawLine.ForEach(o =>
        {
            o.GetComponent<ToggleTools>().posInit = o.anchoredPosition;
        });
        yield return new WaitForSeconds(0.1f);
        groupTools.enabled = false;
        yield return new WaitForSeconds(0.1f);
        listToolDrawLine.ForEach(o =>
        {
            o.GetComponent<ToggleTools>().offSetX = o.anchoredPosition.x;
        });
        SwitchToolsDrawLine();
    }

    public void SwitchToolsDrawLine()
    {
        listToolDrawLine.ForEach(o =>
        {
            ToggleTools toggleColors = o.GetComponent<ToggleTools>();
            toggleColors.MoveTool(toggleColors.offSetX);
            // if(toggleColors.toggleTool.isOn)
            //     listToolDrawLine[0].GetComponent<ToggleTools>().toggleTool.isOn = true;
        });
        listToolDrawFull.ForEach(o =>
        {
            ToggleTools toggleColors = o.GetComponent<ToggleTools>();
            toggleColors.MoveTool(toggleColors.offSetX);
            toggleColors.FadeTool(0, false);
            if(toggleColors.toggleTool.isOn)
                listToolDrawLine[0].GetComponent<ToggleTools>().toggleTool.isOn = true;
        });
    }

    public void SwitchToolsDrawFull()
    {
        listToolDrawLine.ForEach(o =>
        {
            ToggleTools toggleColors = o.GetComponent<ToggleTools>();
            toggleColors.MoveTool(toggleColors.posInit.x);
        });
        listToolDrawFull.ForEach(o =>
        {
            o.gameObject.SetActive(true);
            ToggleTools toggleColors = o.GetComponent<ToggleTools>();
            toggleColors.MoveTool(toggleColors.posInit.x);
            toggleColors.FadeTool(1, true);
        });
    }

    public void EnableAndDisableToolDrawLine(bool status)
    {
        listToolDrawLine.ForEach(o =>
        {
            ToggleTools toggleColors = o.GetComponent<ToggleTools>();
            toggleColors.toggleTool.interactable = status;
        });
    }
}