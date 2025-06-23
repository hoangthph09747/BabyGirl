//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushPony : MonoBehaviour
    {
        [SerializeField] Transform charPos;
        [SerializeField] float range = 8;
        [SerializeField] float speed = 2f;

        private float minX = -5f; // Giới hạn bên trái
        private float maxX = 5f;  // Giới hạn bên phải
        private int direction = -1; // Hướng di chuyển: 1 là sang phải, -1 là sang trái
        private bool canMove = true;
        private Collider2D col;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
        }

        private void Start()
        {
            minX = transform.position.x - range;
            maxX = transform.position.x + range;
            if (range == 0) canMove = false;
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
                localScale.x = Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }
            else if (newX < minX)
            {
                newX = minX;
                direction = 1; // Đổi hướng sang phải
                Vector3 localScale = transform.localScale;
                localScale.x = -Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }

            // Cập nhật vị trí mới
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(ToiletRushManager.CharacterTag)) return;
            if (!collision.TryGetComponent(out ToiletRushCharacter character)) return;
            col.enabled = false;
            canMove = false;

           // transform.DOMove(character.CrabPos.position, .5f);
            character.transform.SetParent(charPos, true);
            character.transform.localScale = new Vector3(-Mathf.Abs(character.transform.localScale.x), character.transform.localScale.y, character.transform.localScale.z);
           // character.transform.DOLocalRotateQuaternion(Quaternion.identity, .5f);
           // character.transform.DOLocalMove(Vector3.zero, .5f);
            character.TouchPony();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.right * range, transform.position + Vector3.left * range);
        }
    }

}

