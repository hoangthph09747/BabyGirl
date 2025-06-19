using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuanUtilities
{
    public static class Utilities
    {
        public static Coroutine DelayFunction(this MonoBehaviour mono, float delay, Action onDone)
        {
            return mono.StartCoroutine(StartDelayFuction(delay, onDone));
        }

        private static IEnumerator StartDelayFuction(float delay, Action onDone)
        {
            yield return new WaitForSeconds(delay);
            onDone?.Invoke();
        }

        public static Coroutine DelayFuctionRealtime(this MonoBehaviour mono, float delay, Action onDone)
        {
            return mono.StartCoroutine(StartDelayFuctionRealtime(delay, onDone));
        }

        private static IEnumerator StartDelayFuctionRealtime(float delay, Action onDone)
        {
            yield return new WaitForSecondsRealtime(delay);
            onDone?.Invoke();
        }

        public static Coroutine WaitUntil(this MonoBehaviour mono, Coroutine coroutine, Action onDone)
        {
            return mono.StartCoroutine(WaitUntilCorotine(coroutine, onDone));
        }
        public static Coroutine WaitUntil(this MonoBehaviour mono, IEnumerator enumerator, Action onDone)
        {
            return mono.StartCoroutine(WaitUntilCorotine(mono.StartCoroutine(enumerator), onDone));
        }

        private static IEnumerator WaitUntilCorotine(Coroutine coroutine, Action onDone)
        {
            yield return coroutine;
            onDone?.Invoke();
        }

        public static Coroutine DelayToNextFrame(this MonoBehaviour mono, Action onDone)
        {
            return mono.StartCoroutine(DelayNextFrameCorotine(onDone));
        }

        private static IEnumerator DelayNextFrameCorotine(Action onDone)
        {
            yield return null; onDone?.Invoke();
        }

        public static void SaveData(string data, string fileName)
        {
            string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

            File.WriteAllText(dataPath, data);
        }
        public static string LoadData(string fileName)
        {
            string dataPath = $"{Application.persistentDataPath}/{fileName}.txt";

            if (File.Exists(dataPath))
                return File.ReadAllText(dataPath);
            else
                return "";
        }

        public static Coroutine RepeatFunction(this MonoBehaviour mono, float time, System.Action onRepeat)
        {
            return mono.StartCoroutine(StartRepeatFuction(time, onRepeat));
        }

        private static IEnumerator StartRepeatFuction(float delay, System.Action onRepeat)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                onRepeat?.Invoke();
            }
        }

        public static T RandomElement<T>(this T[] list)
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }

        public static T RandomElement<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static int RandomElementIndex<T>(this List<T> list)
        {
            return UnityEngine.Random.Range(0, list.Count);
        }

        public static int RandomElementIndex<T>(this T[] list)
        {
            return UnityEngine.Random.Range(0, list.Length);
        }

        public static void RandomFuction(params Action[] funcs)
        {
            int index = RandomElementIndex(funcs);
            funcs[index].Invoke();
        }

        /// <summary>
        /// Use this if EventSystem.current.IsPointerOverGameObject() not work, this often occur in mobile
        /// </summary>
        /// <returns></returns>

        public static bool IsPointerOverUIObject(this MonoBehaviour mono)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static float HalfWidth(this Camera cam)
        {
            return cam.orthographicSize * cam.aspect;
        }
        public static float HalfHeight(this Camera cam)
        {
            return cam.orthographicSize;
        }

        public static bool IsOverlapBox2D(this BoxCollider2D box1, BoxCollider2D box2)
        {
            return box1.bounds.Intersects(box2.bounds);
        }

        public static float BottomBorder(this Camera cam)
        {
            return cam.transform.position.y - cam.orthographicSize;
        }
        public static float TopBorder(this Camera cam)
        {
            return cam.transform.position.y + cam.orthographicSize;
        }
        public static float RightBorder(this Camera cam)
        {
            return cam.transform.position.x + cam.orthographicSize * cam.aspect;
        }
        public static float LeftBorder(this Camera cam)
        {
            return cam.transform.position.x - cam.orthographicSize * cam.aspect;
        }

        public static Vector2 UpRightCorner(this Camera cam)
        {
            return new Vector2(RightBorder(cam), TopBorder(cam));
        }
        public static Vector2 UpLeftCorner(this Camera cam)
        {
            return new Vector2(LeftBorder(cam), TopBorder(cam));
        }
        public static Vector2 DownLeftCorner(this Camera cam)
        {
            return new Vector2(LeftBorder(cam), BottomBorder(cam));
        }
        public static Vector2 DownRightCorner(this Camera cam)
        {
            return new Vector2(RightBorder(cam), BottomBorder(cam));
        }
    }

}

