using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(SimpleCollider))]
public class SimpleColliderEvent : MonoBehaviour
{
    public MouseEvent onMouseDown;
    public MouseEvent onMouseDrag;
    public MouseEvent onMouseUp;
    [Serializable] public class MouseEvent : UnityEvent<Vector3> { }
    private Camera mainCam;
    private SimpleCollider col;

    private void Awake()
    {
        col = GetComponent<SimpleCollider>();
        mainCam = Camera.main;
    }

    bool isClick;
    private void Update()
    {
        Vector3 mousePos = MousePos();
        if (Input.GetMouseButtonDown(0))
        {
            if (col.CheckPoint(mousePos))
            {
                onMouseDown?.Invoke(mousePos);
                isClick = true;
            }
        }
        if (Input.GetMouseButton(0))
            if (isClick)
            {
                if (col.CheckPoint(mousePos))
                    onMouseDrag?.Invoke(mousePos);
                else
                {
                    isClick = false;
                    onMouseUp?.Invoke(mousePos);
                }
            }

        if (Input.GetMouseButtonUp(0))
        {
            if (isClick)
            {
                isClick = false;
                onMouseUp?.Invoke(mousePos);
            }
        }
    }

    private Vector3 MousePos()
    {
        Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}
