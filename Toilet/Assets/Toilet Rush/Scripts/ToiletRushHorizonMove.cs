using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushHorizonMove : MonoBehaviour
    {
        [SerializeField] float range = 15;
        [SerializeField] float speed = 2f;
        [SerializeField] int rightScale = 1;

        private float minX = -5f; // Giới hạn bên trái
        private float maxX = 5f;  // Giới hạn bên phải
        private int direction = -1; // Hướng di chuyển: 1 là sang phải, -1 là sang trái
        private bool canMove = true;
        public float Range => range;

        private void Start()
        {
            minX = transform.position.x - range;
            maxX = transform.position.x + range;
            if (range == 0) canMove = false;

            Vector3 localScale = transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x) * rightScale;
            transform.localScale = localScale;
        }

        private void Update()
        {
            if (!canMove) return;

            float newX = transform.position.x + direction * speed * Time.deltaTime;

            if (newX > maxX)
            {
                newX = maxX;
                direction = -1; // Đổi hướng sang trái
                Vector3 localScale = transform.localScale;
                localScale.x = -Mathf.Abs(localScale.x) * rightScale;
                transform.localScale = localScale;
            }
            else if (newX < minX)
            {
                newX = minX;
                direction = 1; // Đổi hướng sang phải
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * rightScale;
                transform.localScale = localScale;
            }

            // Cập nhật vị trí mới
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }

        public void SetCanMove(bool canMove)
        {
            this.canMove = canMove;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.right * range, transform.position + Vector3.left * range);
        }
    }
}

