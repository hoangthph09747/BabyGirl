using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtendedAnimation
{
    public class CircleMoveEffect : MonoBehaviour
    {
        [SerializeField] private Vector3 center;
        [SerializeField] private Transform target;
        [SerializeField] private float speed;

        IEnumerator circleMoveCorotine;

        private float radius;

        float defaultAngle = 90;

        private void Start()
        {
            defaultAngle = Vector3.Angle(target.position - center, Vector3.right);
            radius = (target.position - center).magnitude;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                MoveAround(center,speed,-1);
        }

        public void MoveAround(Vector3 center, float speed, float time)
        {
            defaultAngle = Vector3.Angle(target.position - center, Vector3.right);
            radius= Vector3.Distance(center, target.transform.position);

            if (time < 0)
            {
                circleMoveCorotine = StartInfMove(center, speed);
                StartCoroutine(circleMoveCorotine);
            }
            else if (time > 0)
            {

            }
        }

        private IEnumerator StartInfMove(Vector3 center, float speed)
        {
            float x = 0, y = 0;
            float angle = defaultAngle;
            while(true)
            {
                x = Mathf.Cos(angle) * radius+center.x;
                y = Mathf.Sin(angle) * radius+center.y;
                target.transform.position = new Vector3(x, y);
                angle+=speed*Time.deltaTime;
                Debug.Log(x);
            }    
        }
        public void StopMoveAround()
        {
            if (circleMoveCorotine != null)
            {
                StopCoroutine(circleMoveCorotine);
                circleMoveCorotine = null;
            }
        }
    }
}