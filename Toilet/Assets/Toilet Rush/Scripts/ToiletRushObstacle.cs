using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushObstacle : MonoBehaviour
    {
        private void Awake()
        {
            tag = ToiletRushManager.ObstacleTag;
        }
    }
}
