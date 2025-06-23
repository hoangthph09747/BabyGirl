using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInsatnceMat : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            var mat = renderer.material;
        }
    }
}
