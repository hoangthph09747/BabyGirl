using System;
using UnityEngine;
using UnityEngine.EventSystems;
using PrincessColoring;
public class CameraController_Move : MonoBehaviour
{
    public float cameraSpeed = 0.1f;
    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 10f;
    public float minY = -10f;
    public bool moveAble = true;
    private Vector3 dragOrigin;
    Camera camera;
    SliderZoomCamera sliderZoom;
    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();

        maxX += transform.parent.position.x;
        minX += transform.parent.position.x;
        maxY += transform.parent.position.y;
        minY += transform.parent.position.y;

        sliderZoom = FindObjectOfType<SliderZoomCamera>();
    }

    private void OnSliderValueChanged(float arg0)
    {
        moveAble = false;
    }

    public bool IsAnyPointerOverGameObject()
    {
        bool result = EventSystem.current.IsPointerOverGameObject();
        foreach (var touch in Input.touches)
        {
            result |= EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }

        return result;
    }

    void LateUpdate()
    {
        if (IsAnyPointerOverGameObject() || !moveAble || sliderZoom.isDragging)
            return;


#if UNITY_EDITOR
        MoveFollowMouse();
#else
        MoveFollowTouch();
#endif
    }

    Touch touch;

    void MoveFollowMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * cameraSpeed, pos.y * cameraSpeed, 0);

        transform.position -= move;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z);
    }    
    void MoveFollowTouch()
    {
        if(Input.touchCount == 1)
        {
            touch = Input.touches[0];
            if(touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                dragOrigin = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                Vector3 pos = camera.ScreenToViewportPoint((Vector3)touch.position - dragOrigin);
                Vector3 move = new Vector3(pos.x * cameraSpeed, pos.y * cameraSpeed, 0);

                transform.position -= move;

                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    Mathf.Clamp(transform.position.y, minY, maxY),
                    transform.position.z);
            }    
        }    
    }    
}
