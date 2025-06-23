using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushVerticalMove : MonoBehaviour
    {
        [SerializeField] float range = 8;
        [SerializeField] float speed = 2f;

        private float minY = -5f; // Giới hạn bên trái
        private float maxY = 5f;  // Giới hạn bên phải
        private int direction = -1; // Hướng di chuyển: 1 là sang phải, -1 là sang trái
        public bool canMove = true;

        private void Start()
        {
            minY = transform.position.y - range;
            maxY = transform.position.y + range;
            if (range == 0) canMove = false;
        }

        private void Update()
        {
            if (!canMove) return;

            float newY = transform.position.y + direction * speed * Time.deltaTime;

            if (newY > maxY)
            {
                newY = maxY;
                direction = -1; // Đổi hướng sang trái
            }
            else if (newY < minY)
            {
                newY = minY;
                direction = 1; // Đổi hướng sang phải
            }

            // Cập nhật vị trí mới
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.down * range, transform.position + Vector3.up * range);
        }
    }
}
