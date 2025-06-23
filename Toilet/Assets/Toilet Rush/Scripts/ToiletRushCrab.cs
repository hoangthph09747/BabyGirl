//using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToiletRush
{
    public class ToiletRushCrab : MonoBehaviour
    {
        [SerializeField] float range = 8;
        [SerializeField] float speed = 2f;
        [SerializeField] AudioClip soundEffect;

        private float minX = -5f; // Giới hạn bên trái
        private float maxX = 5f;  // Giới hạn bên phải
        private int direction = -1; // Hướng di chuyển: 1 là sang phải, -1 là sang trái
        private bool canMove = true;
        private SkeletonAnimation anim;
        private Collider2D col;

        private void Awake()
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
            col = GetComponent<Collider2D>();
        }

        private void Start()
        {
            minX = transform.position.x - range;
            maxX = transform.position.x + range;
            if(range == 0) canMove = false;
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
            anim.state.SetAnimation(0, "idle", true);
            canMove = false;

            //transform.DOMove(character.CrabPos.position, .5f);
            SoundManager_BabyGirl.Instance.DoMove(transform, character.CrabPos.position, .5f);
            SoundManager_BabyGirl.Instance.PlaySoundEffectOneShot(soundEffect);
            character.TouchCrab();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.right * range, transform.position + Vector3.left * range);
        }
    }
}
