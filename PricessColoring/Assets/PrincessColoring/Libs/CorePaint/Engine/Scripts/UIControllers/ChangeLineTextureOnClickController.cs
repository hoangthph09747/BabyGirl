using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintCraft.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using PaintCraft.Controllers;


namespace PatinCraft.UI{    
    public class ChangeLineTextureOnClickController : MonoBehaviour, IPointerClickHandler {
        public LineConfig LineConfig;
        public Texture Texture;
		private ScreenCameraController screenCameraController;
		private GameObject panelStamp;

		private void Start()
		{
			screenCameraController = FindObjectOfType<ScreenCameraController>();
			panelStamp = GameObject.Find("PanelStamps");
		}

		#region IPointerClickHandler implementation
		public void OnPointerClick(PointerEventData eventData)
        {
            //GameController.instance.listToggleGroup.ForEach((obj) => obj.SetAllTogglesOff());
            screenCameraController.LineConfig = LineConfig;
            LineConfig.Texture = Texture;
			StartCoroutine(IEHiddenPanelStamp());
        }
		#endregion

		IEnumerator IEHiddenPanelStamp()
		{
			yield return new WaitForSeconds(0.2f);
			panelStamp.GetComponent<RectTransform>().DOScale(new Vector3(0, 0), 0.2f);

		}

        [ContextMenu("setup icon")]
        public void SetupIcon(){
            Object.DestroyImmediate( gameObject.transform.GetChild(0).gameObject.GetComponent<Text>());
            gameObject.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = Texture;
        }
    }
}