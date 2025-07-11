using UnityEngine;
using UnityEngine.EventSystems;
using PaintCraft.Canvas;
using PaintCraft.Tools;
using System.Collections.Generic;
using UnityEngine.Serialization;
using PaintCraft;
using UnityEngine.Rendering;
using System.Collections;
using PaintCraft.Canvas.Configs;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

namespace PaintCraft.Controllers
{
    [RequireComponent(typeof(Camera))]
    public class ScreenCameraController : InputController
    {
        public static ScreenCameraController instance;
        [HideInInspector] public Scene scene;
        public LineConfig LineConfig;
        public Camera Camera { get; private set; }
        public bool ZoomOnMouseScroll = false;
        public CameraSizeHandler CameraSize;

        [Header("Other")] public bool isDrawing;
        public Vector3 worldPosition;
        private CameraDrag cameraDrag;
        private PinchZoom pinchZoom;
        CommandBuffer commandBuffer;


        public CommandBuffer CommandBuffer
        {
            get { return commandBuffer; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            scene = SceneManager.GetActiveScene();
            if (Canvas == null)
            {
                Debug.LogError("you have to provide link to the Canvas for this component", gameObject);
            }

            Camera = GetComponent<Camera>();
            if (Camera == null)
            {
                Debug.Log("you have to add camera component to this object", gameObject);
            }

            CameraSize.Init(Camera, Canvas);

            cameraDrag = GetComponent<CameraDrag>();
            pinchZoom = GetComponent<PinchZoom>();
            commandBuffer = new CommandBuffer();
            commandBuffer.name = gameObject.name;
            Camera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
            Canvas.OnPageChange += OnPageChangeHandler;
        }

        void OnDestroy()
        {
            Canvas.OnPageChange -= OnPageChangeHandler;
            instance = null;
        }

        void OnPageChangeHandler(PageConfig newPage)
        {
            CameraSize.ResetSize();
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

        RaycastHit hit;
        void Update()
        {

            ////ngocdu
            //Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out hit))
            //{
            //    // Kiểm tra xem Raycast chạm vào collider của vật
            //    if (hit.collider != null)
            //    {
            //        Debug.LogError("hit: " + hit.collider.name);
            //        if(hit.collider.gameObject.GetComponent<BackLayerController>())
            //        {
                
            //        }    
            //    }
            //}

            //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
            //{
            //    if (DIY.GameManager_DIY.Instance.drawing == false)
            //        return;
            //}    

            if (EventSystem.current != null && IsAnyPointerOverGameObject() == false)
            {
#if UNITY_EDITOR
                HandleMouseEvents();

#elif !UNITY_EDITOR
                    HandleTouch();
#endif
            }

            CameraSize.LateUpdate();
        }

        /// <summary>
        /// kiểm tra xem có chạm vào bức tranh hay không
        /// </summary>
        /// <returns></returns>
        bool CheckTouchOnPage()
        {
            //ngocdu
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra xem Raycast chạm vào collider của vật
                if (hit.collider != null)
                {
                    //Debug.LogError("hit: " + hit.collider.name);
                    if (hit.collider.gameObject.GetComponent<BackLayerController>())
                    {
                        //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.DIY)
                        //{
                            
                        //    HandGuidleDraw.instance.Hide();
                        //    if(DIY.GameManager_DIY.Instance.indexTutorial == 3)
                        //    {
                        //        DIY.GameManager_DIY.Instance.ShowTutorialScroll();
                        //    }    
                        //}    
                        return true;
                    }
                }
            }

            return false;
        }    

        void OnEnable()
        {
            StartCoroutine("ClearCommandBuffer");
        }

        void OnDisable()
        {
            StopCoroutine("ClearCommandBuffer");
        }

        IEnumerator ClearCommandBuffer()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (CommandBuffer != null)
                {
                    CommandBuffer.Clear();
                }
            }
        }


        void HandleZoomOnScroll()
        {
            if (ZoomOnMouseScroll && Camera.pixelRect.Contains(Input.mousePosition))
            {
                float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel") * 100.0f;
                if (mouseScrollWheel != 0.0f)
                {
                    CameraSize.SetCameraNewOrthoSize(Camera.orthographicSize - mouseScrollWheel);
                    CameraSize.CheckBounds();
                }
            }
        }

        /// <summary>
        /// Handles the touch.
        /// </summary>
        /// <returns><c>true</c>, if touch was handled, <c>false</c> otherwise.</returns>
        void HandleTouch()
        {
            if (Input.touchCount > 1)
                return;

            if (Input.touchCount == 1)
            {
                foreach (var touch in Input.touches)
                {
                    worldPosition = GetWorldPosition(touch.position);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            posBeginTouch = touch.position;
                            ToolsType toolsType = ToolsType.Colors;
                            //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                            //{
                            //    toolsType = DrawPictureController.Instance.ToolsType;
                            //    DrawPictureController.Instance.PlaySoundTouch();
                            //}
                            //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                            //{
                            //    toolsType = DrawPictureControllerASMR.Instance.ToolsType;
                            //    DrawPictureControllerASMR.Instance.PlaySoundTouch();
                            //}
                            toolsType = DrawPictureController.Instance.ToolsType;
                            DrawPictureController.Instance.PlaySoundTouch();
                            switch (toolsType)
                            {
                                case ToolsType.Stamps:
                                    // SoundManager.Instance?.PlayUISound(SoundType.Stamp);
                                    break;
                                case ToolsType.Fills:
                                    // SoundManager.Instance?.PlayUISound(SoundType.Bucket);
                                    break;
                            }
                            if( CheckTouchOnPage())
                            BeginLine(LineConfig, touch.fingerId, worldPosition, true);
                            break;
                        case TouchPhase.Moved:
                            if (CheckTouchOnPage())
                                ContinueLine(0, worldPosition);
                            break;
                        case TouchPhase.Stationary:
                            break;
                        case TouchPhase.Canceled:
                        case TouchPhase.Ended:
                            if (CheckTouchOnPage())
                            {
                                posEndTouch = touch.position;
                                if (CheckDistanceTouchBeginAndEnd())
                                {
                                    ToolsType toolsType1 = ToolsType.Colors;
                                    //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                                    //{
                                    //    toolsType1 = DrawPictureController.Instance.ToolsType;
                                    //}
                                    //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                                    //{
                                    //    toolsType1 = DrawPictureControllerASMR.Instance.ToolsType;
                                    //}
                                    toolsType1 = DrawPictureController.Instance.ToolsType;
                                    if (toolsType1 == ToolsType.Fills || toolsType1 == ToolsType.Stamps)
                                        EndLineOneTouch(0, worldPosition);
                                    else
                                        EndLine(0, worldPosition);
                                    //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[UnityEngine.Random.Range(8, 10)]);
                                }
                            }
                            break;
                    }
                }
            }
        }

        Vector2 posBeginTouch, posEndTouch;
        /// <summary>
        /// kiểm tra nếu điểm bắt đầu và kết thúc xa nhau quá thì ko cho vẽ vì lúc đó user đang kéo tranh chứ ko muốn vẽ
        /// </summary>
        bool CheckDistanceTouchBeginAndEnd()
        {
            if(Vector2.Distance(posBeginTouch, posEndTouch) > 30)
            {
                return false;
            }
            return true;
        }    
        void HandleMouseEvents()
        {
            worldPosition = GetWorldPosition(Input.mousePosition);
            if (!isDrawing)
            {
                if (Input.GetMouseButtonDown(0) && CheckTouchOnPage())
                {
                    posBeginTouch = Input.mousePosition;
                    ToolsType toolsType = ToolsType.Colors;
                    //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                    //{
                    //    toolsType = DrawPictureController.Instance.ToolsType;
                    //    DrawPictureController.Instance.PlaySoundTouch();
                    //}
                    //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                    //{
                    //    toolsType = DrawPictureControllerASMR.Instance.ToolsType;
                    //    DrawPictureControllerASMR.Instance.PlaySoundTouch();
                    //}
                    toolsType = DrawPictureController.Instance.ToolsType;
                    DrawPictureController.Instance.PlaySoundTouch();
                    switch (toolsType)
                    {
                        case ToolsType.Stamps:
                            // SoundManager.Instance?.PlayUISound(SoundType.Stamp);
                            break;
                        case ToolsType.Fills:
                            // SoundManager.Instance?.PlayUISound(SoundType.Bucket);
                            break;
                    }

                    BeginLine(LineConfig, 0, worldPosition, true);
                }
                else if (Input.GetMouseButton(0) && CheckTouchOnPage())
                {
                    ContinueLine(0, worldPosition);
                }
                else if (Input.GetMouseButtonUp(0) && CheckTouchOnPage())
                {
                    posEndTouch = Input.mousePosition;
                    if (CheckDistanceTouchBeginAndEnd())
                    {
                        ToolsType toolsType1 = ToolsType.Colors;
                        //if (LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
                        //{
                        //    toolsType1 = DrawPictureController.Instance.ToolsType;
                        //}
                        //else if (LoadSceneManager.Instance.nameMinigame == NameMinigame.ColoringASMR)
                        //{
                        //    toolsType1 = DrawPictureControllerASMR.Instance.ToolsType;
                        //}
                        toolsType1 = DrawPictureController.Instance.ToolsType;
                        if (toolsType1 == ToolsType.Fills || toolsType1 == ToolsType.Stamps)
                            EndLineOneTouch(0, worldPosition);
                        else
                            EndLine(0, worldPosition);

                        //ToySoundManager.instance.PlaySoundEffect(ToySoundManager.instance.allSoundEffect[UnityEngine.Random.Range(8, 10)]);
                    }
                }
            }
        }

        public override bool DontAllowInteraction(Vector2 worldPosition)
        {
            bool result = !Camera.pixelRect.Contains(Camera.WorldToScreenPoint(worldPosition));
            result |= IsAnyPointerOverGameObject();
            return result;
        }

        Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Vector3 vector3ScreenPosition = screenPosition;
            vector3ScreenPosition.z = transform.position.z;
            return Camera.ScreenToWorldPoint(vector3ScreenPosition);
        }
        
    }
}