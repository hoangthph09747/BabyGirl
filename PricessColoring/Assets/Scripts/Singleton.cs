using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"[Singleton<{typeof(T)}>] instance is null in scene. Make sure it's in the scene.");

                    //GameObject newGO = new GameObject(typeof(T).Name);
                    //instance = newGO.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Ngăn trùng lặp singleton
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
