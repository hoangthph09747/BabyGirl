using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTilingX : MonoBehaviour
{
    private void Start()
    {
        var mat = GetComponent<Renderer>().material;
        mat.SetVector("_Tiling_Value", new Vector2(transform.localScale.x, mat.GetVector("_Tiling_Value").y));
    }
}
