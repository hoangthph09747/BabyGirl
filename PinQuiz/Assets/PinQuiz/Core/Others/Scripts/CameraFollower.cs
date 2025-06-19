using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HongQuan
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] Transform follow;
        [SerializeField] float damping = 10;
        Transform _transform;
        Vector3 offset;
        private void Awake()
        {
            _transform = transform;
            offset = transform.position - follow.position;
        }

        private void Update()
        {
            _transform.position = Vector3.Lerp(_transform.position, follow.position + offset, damping * Time.deltaTime);
        }
    }

}