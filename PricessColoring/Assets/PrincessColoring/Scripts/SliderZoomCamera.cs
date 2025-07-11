using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace PrincessColoring
{
    public class SliderZoomCamera : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Slider slider;
        public bool isDragging = false;
        [SerializeField]
        AudioSource audioSourceIncreased, audioSourceDecreased;
        private float previousValue; // Lưu trữ giá trị trước đó của Slider

        private void Awake()
        {
            StopSound();
        }
        private void Start()
        {
            if (slider == null)
            {
                slider = GetComponent<Slider>();
            }
            //if(LoadSceneManager.Instance.nameMinigame == NameMinigame.PrincessColoring)
            //slider.DOValue(820, 1).OnComplete(() => {
            //    StopSound();
            //});
            //else
            //    slider.DOValue(500, 1).OnComplete(()=> {
            //        StopSound();
            //    });

            slider.DOValue(820, 1).OnComplete(() => {
                StopSound();
            });
            // Add a listener to the Slider's onValueChanged event.
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Bắt đầu kéo slider
            isDragging = true;
            //Debug.Log("Bắt đầu kéo slider");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Kết thúc kéo slider
            isDragging = false;
            //Debug.Log("Kết thúc kéo slider");

            StopSound();
        }

        private void OnSliderValueChanged(float currentValue)
        {
            if (currentValue > previousValue)
            {
                Debug.Log("Slider value increased");
                
                audioSourceIncreased.volume = 1;
                audioSourceDecreased.volume = 0;
            }
            else if (currentValue < previousValue)
            {
                Debug.Log("Slider value decreased");
                audioSourceIncreased.volume = 0;
                audioSourceDecreased.volume = 1;
            }
            else
            {
                Debug.Log("Slider value unchanged");
                StopSound();
            }
            previousValue = currentValue; // Cập nhật giá trị trước đó
        }

        public void StopSound()
        {
            if (audioSourceIncreased)
            {
                audioSourceIncreased.volume = 0;
                audioSourceDecreased.volume = 0;
            }
        }    
    }
}
