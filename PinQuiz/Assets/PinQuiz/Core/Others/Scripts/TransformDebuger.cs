using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan {
#if UNITY_EDITOR
    public class TransformDebuger : MonoBehaviour
    {
        void Update()
        {
            string msg = name + "\nPosition = " + transform.position.ToString() + "\n";
            msg +=  "Local Position = " + transform.localPosition.ToString();
            print(msg);
        }
    }
#endif
}
