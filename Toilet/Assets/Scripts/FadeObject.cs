using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan {
    public class FadeObject : MonoBehaviour
    {
        public virtual void Fade(float to, float duration = 0.5f, System.Action onDone = null)
        {

        }

        public virtual void Fade(float from, float to, float duration = 0.5f, System.Action onDone = null)
        {

        }

    }
}