using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Observer;
using System;
using DG.Tweening;
namespace PinQuiz
{
    public class PinQuizCoin : PinQuizEntity
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        Action<object> _OnWin;
        protected override void Start()
        {
            PinQuizManager.instance.AddCoin(this);
            RandomSprite();
            transform.localScale = Vector3.one;

            _OnWin = (param) => OnWinPinQuizPrincess((GameObject)param);
            this.RegisterListener(EventID.OnWinPinQuizPrincess, _OnWin);
        }

        private void OnDestroy()
        {
            this.RemoveListener(EventID.OnWinPinQuizPrincess, _OnWin);
            _OnWin = null;
            if (PinQuizManager.instance != null)
                PinQuizManager.instance.RemoveCoin(this);
        }

        private void OnWinPinQuizPrincess(GameObject param)
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            //transform.DOMove(new Vector3(param.transform.position.x, param.transform.position.y + 1, param.transform.position.z), 1).
            //    SetEase(Ease.InOutBack);
            List<Vector3> listPos = new List<Vector3>();
            listPos.Add(transform.position);
            if (transform.position.x > param.transform.position.x)
                listPos.Add(new Vector3(param.transform.position.x + 1, param.transform.position.y + 5, param.transform.position.z));
            else
                listPos.Add(new Vector3(param.transform.position.x - 1, param.transform.position.y + 5, param.transform.position.z));
            listPos.Add(new Vector3(param.transform.position.x, param.transform.position.y + 1, param.transform.position.z));
            transform.DOPath(listPos.ToArray(), Vector2.Distance(transform.position, param.transform.position) / 2, PathType.CatmullRom).
                SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    Vector3 pos = new Vector3(param.transform.position.x + UnityEngine.Random.Range(-1.1f, 1.1f), param.transform.position.y + UnityEngine.Random.Range(0.1f, 3.0f), param.transform.position.z);
                    PinQuizManager.instance.ShowEff(1.75f, pos, PinQuizManager.TypeEff.CoinCollect);
                    /*GameObject effect = Instantiate(PinQuizManager.instance.effectCoinCollect);
                    effect.transform.position = new Vector3(param.transform.position.x + UnityEngine.Random.Range(-1.1f, 1.1f), param.transform.position.y + UnityEngine.Random.Range(0.1f, 3.0f), param.transform.position.z);
                    effect.transform.localScale = Vector3.one * 0.5f;
                    effect.SetActive(true);
                    Destroy(effect, 3);*/

                    SoundManager_BabyGirl.Instance.PlayOneShot($"Sounds/unlock");
                    Destroy(gameObject);
                });
            //transform.DOScale(Vector2.zero, Vector2.Distance(transform.position, param.transform.position) / 2).SetEase(Ease.Linear);
        }

        private void RandomSprite()
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            if (PinQuizManager.instance.CoinSprites.Length < 2) return;

            if (r < .7f)
            {
                spriteRenderer.sprite = PinQuizManager.instance.CoinSprites[0];
            }
            else
            {
                spriteRenderer.sprite = PinQuizManager.instance.CoinSprites[UnityEngine.Random.Range(1, PinQuizManager.instance.CoinSprites.Length)];
            }
        }

        public override void Die()
        {
            PinQuizManager.instance.RemoveCoin(this);
            base.Die();
        }

        public override void Die(float delay)
        {
            base.Die(delay);
            PinQuizManager.instance.princess.TreasureDestroyed();
        }

        public override void BeingExplosed(PinQuizTNT bomb)
        {
            PinQuizManager.instance.princess.TreasureDestroyed();
            base.BeingExplosed(bomb);
            Die();
        }

        public override void TouchLava(PinQuizLava lava)
        {
            base.TouchLava(lava);
            PinQuizManager.instance.princess.LavaTouchTreasure();
            Die();
        }

        public override void TouchSlime()
        {
            base.TouchSlime();
            PinQuizManager.instance.princess.SlimeTouchTreasure();
            Die();
        }

        public void DisablePhysic()
        {
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }

        public void DisableRenderer()
        {
            spriteRenderer.enabled = false;
        }

        public void FadeRenderer(float endValue, float duration)
        {
            spriteRenderer.DOFade(endValue, duration);
        }
    }
}