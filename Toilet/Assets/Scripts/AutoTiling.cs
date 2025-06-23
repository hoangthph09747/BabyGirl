using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    [RequireComponent(typeof(Renderer))]
    public class AutoTiling : MonoBehaviour
    {
        public void Start()
        {
            var mat = GetComponent<Renderer>().material;
            mat.SetVector("_Tiling_Value", new Vector2(transform.localScale.x,transform.localScale.y));
        }
    }
}
