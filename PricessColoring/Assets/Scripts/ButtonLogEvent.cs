using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonLogEvent : MonoBehaviour
{
    Button button;
    [SerializeField]
    string eventName;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (button)
            button.onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Click()
    {
        //if(eventName != "")
        //    BonBonAnalytics.GetInstance().LogEvent(eventName);
        //else
        //    BonBonAnalytics.GetInstance().LogEvent("btn_" + SceneManager.GetActiveScene().name + "_" + gameObject.name);
    }
}
