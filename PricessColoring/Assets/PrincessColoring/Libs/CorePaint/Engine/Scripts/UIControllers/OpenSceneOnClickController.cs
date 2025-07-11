using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif


namespace PatinCraft.UI{ 
    public class OpenSceneOnClickController : MonoBehaviour, IPointerClickHandler {
        public string SceneName;
    
        #region IPointerClickHandler implementation
        public void OnPointerClick(PointerEventData eventData)
        {            
            AppData.SelectedPageConfig = null;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
            SceneManager.LoadScene(SceneName);
#else
            Application.LoadLevel(SceneName);
#endif
        }
        #endregion
    }
}