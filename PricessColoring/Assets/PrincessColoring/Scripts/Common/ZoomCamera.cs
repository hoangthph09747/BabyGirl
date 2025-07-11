using UnityEngine;
using UnityEngine.UI;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] float ZoomMinBound = 0.1f;
    [SerializeField] public float ZoomMaxBound = 179.9f;
    [SerializeField] Camera cam;

    public Slider slider;
    public bool zoomAble = true;

    PrincessColoring.SliderZoomCamera sliderZoomCamera;

    // Use this for initialization
    void Start()
    {
        if(!cam) cam = GetComponent<Camera>();
        
        slider.minValue = ZoomMinBound;
        slider.maxValue = ZoomMaxBound;
        slider.onValueChanged.AddListener(OnSliderValueChanged);

        sliderZoomCamera = slider.GetComponent<PrincessColoring.SliderZoomCamera>();
    }

    private void OnSliderValueChanged(float newValue)
    {
        if(Input.touchCount < 2)
        cam.orthographicSize = 1700 - newValue;
    }

    float zoom;
    float zoomSpeed = 100;
    private void Update()
    {
        if (Input.touchCount == 2 && zoomAble)
        {
            // Lấy thông tin về hai cử chỉ chạm trên màn hình
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Tính toán khoảng cách giữa hai cử chỉ chạm
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Tính toán độ lớn thay đổi khoảng cách giữa hai cử chỉ chạm
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Cập nhật giá trị zoom
            zoom += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;

            // Giới hạn giá trị zoom
            zoom = Mathf.Clamp(zoom, -10f, 10f);

            // Áp dụng zoom vào camera
            if (cam.orthographicSize >= ZoomMinBound && cam.orthographicSize <= ZoomMaxBound)
            {
                cam.orthographicSize += zoom;
                if (cam.orthographicSize > ZoomMaxBound)
                    cam.orthographicSize = ZoomMaxBound;
                else if (cam.orthographicSize < ZoomMinBound)
                    cam.orthographicSize = ZoomMinBound;

                slider.value = 1700 - cam.orthographicSize ;
            }
            zoom = 0f;
        }

        if(Input.GetMouseButtonUp(0))
        {
            sliderZoomCamera.StopSound();
        }    
    }
}