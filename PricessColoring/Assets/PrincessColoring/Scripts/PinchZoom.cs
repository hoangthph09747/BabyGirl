using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintCraft.Controllers;
using PaintCraft.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PinchZoom : MonoBehaviour
{

    public static PinchZoom instance;

    [SerializeField]
    private Camera cameraInput;

    [SerializeField]
    public float perspectiveZoomSpeed = 0.5f;
    // The rate of change of the field of view in perspective mode.
    [SerializeField]
    float orthoZoomSpeed = 0.5f;
    [SerializeField]
    float speedCameraMove = 35;


    public float minOrthoZoom = 10;

    public float maxOrthoZoom = 100;
    [SerializeField]
    float minPerpectiveZoom = 10;
    [SerializeField]
    float maxPerpectiveZoom = 100;

    public Vector2 panLimitMin;
    public Vector2 panLimitMax;
    float prevTouchDeltaMag;
    float touchDeltaMag;
    float distanceBegin;
    float aspect;
    int isEndable = 0;

    Touch touchZero;
    Touch touchOne;

    public bool isZooming;
    public GameObject pen, bodyPen;
    public float sizeCamereBegin;
    public Vector3 positionCameraBegin;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Input.multiTouchEnabled = true;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    // Use this for initialization
    void Start()
    {
        aspect = (float)Screen.width / (float)Screen.height;

        //ngocdu chỉnh độ zoom out camera màn vẽ tranh
        // if(AppData.SelectedPageConfig.GetSize().y > 1024)
        // maxOrthoZoom = 1200;

        cameraInput.orthographicSize = maxOrthoZoom;
        sizeCamereBegin = cameraInput.orthographicSize;
        positionCameraBegin = new Vector3(0, 0, 10);
    }

    private void OnTakeScreenshot()
    {
        cameraInput.orthographicSize = sizeCamereBegin;
        cameraInput.transform.localPosition = positionCameraBegin;
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

    public void CheckTouch()
    {
        if (isEndable == 0)
        {
            distanceBegin = prevTouchDeltaMag;
            isEndable++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current != null && IsAnyPointerOverGameObject() == false)
        {
            ZoomPicture();
            HandleTouch();
        }
        //StartCoroutine(DelayBackDefaultPositionCamera());
    }

    IEnumerator DelayBackDefaultPositionCamera()
    {

        if (cameraInput.orthographicSize >= 410)
        {
            yield return new WaitForSeconds(1);
            cameraInput.transform.position = new Vector3(-18, 0, -100);
            speedCameraMove = 0;
        }
        else
        {
            speedCameraMove = 35;
        }
    }

    public void ZoomPicture()
    {
        if (Input.touchCount <= 1)
        {
            isZooming = false;
            return;
        }
        
        if (Input.touchCount == 2)
        {
            // if (GameController.instance.penBrush)
            // {
            //     pen.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            //     bodyPen.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            //     ScreenCameraController.instance.objectDraw.GetComponent<ObjectDraw>().IsDrawing = false;
            //     ScreenCameraController.instance.isDrawing = true;
            // }
            // else if (GameController.instance.handBrush)
            // {
            //     ScreenCameraController.instance.isDrawing = true;
            // }
            //
            // if (SoundController.instance != null)
            // {
            //     SoundController.instance.gameSoundMusic.Stop();
            // }
            isZooming = true;

            if (isZooming)
            {
                // Store both touches.
                //Touch touchZero = Input.GetTouch(0);
                touchZero = Input.GetTouch(0);
                touchOne = Input.GetTouch(1);
                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                CheckTouch();
                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...


                if (prevTouchDeltaMag < distanceBegin - 100 || prevTouchDeltaMag > distanceBegin + 100)
                {
                    if (cameraInput.orthographic)
                    {

                        // ... change the orthographic size based on the change in distance between the touches.
                        cameraInput.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                        // Make sure the orthographic size never drops below zero.
                        cameraInput.orthographicSize = Mathf.Clamp(cameraInput.orthographicSize, minOrthoZoom, maxOrthoZoom);
                    }
                    else
                    {

                        // Otherwise change the field of view based on the change in distance between the touches.
                        cameraInput.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                        // Clamp the field of view to make sure it's between 0 and 180.
                        cameraInput.fieldOfView = Mathf.Clamp(cameraInput.fieldOfView, minPerpectiveZoom, maxPerpectiveZoom);
                    }
                }

                // just move the camera
                Vector3 newPos = cameraInput.transform.position - (Vector3)touchZero.deltaPosition * speedCameraMove * Time.deltaTime;

                // clamp pos, NOTE: doesnt take account of zooming yet
                newPos.x = Mathf.Clamp(newPos.x, panLimitMin.x, panLimitMax.x);
                newPos.y = Mathf.Clamp(newPos.y, panLimitMin.y, panLimitMax.y);

                cameraInput.transform.position = newPos;

            }

            TouchHandle();
        }

    }

    public void TouchHandle()
    {

        if (Input.touchCount == 2)
        {

            if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Ended)
            {
                Debug.Log("SET ENABLE = 0");
                isEndable = 0;
            }

        }


    }

    void HandleTouch()
    {
        if (Input.touchCount <= 1)
        {
            foreach (var touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Ended:

                        // ScreenCameraController.instance.isDrawing = false;

                        isZooming = false;
                        break;
                }
            }
        }
    }




}
