using UnityEngine;

namespace PinQuiz
{
    public class PinQuizSlimeHandler : MonoBehaviour
    {
        private void Start()
        {
            PinQuizManager.instance.SetActiveSlimeCamera(true);
        }
    }
}
