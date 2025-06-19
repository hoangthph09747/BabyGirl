using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HongQuan
{
    public static class InputUtiis
    {
        public static bool OnTouchDown()
        {
#if ENABLE_INPUT_SYSTEM
#if UNITY_EDITOR
            return Mouse.current.leftButton.wasPressedThisFrame;
#else
            return Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
#endif
#else
            return Input.GetMouseButtonDown(0);
#endif

        }
        public static bool OnTouchHold()
        {
#if ENABLE_INPUT_SYSTEM
#if UNITY_EDITOR
            return Mouse.current.leftButton.isPressed;
#else
            return Touchscreen.current.primaryTouch.press.isPressed;
#endif
#else
            return Input.GetMouseButton(0);
#endif
        }
        public static bool OnTouchRealse()
        {
#if ENABLE_INPUT_SYSTEM
#if UNITY_EDITOR
            return Mouse.current.leftButton.wasReleasedThisFrame;
#else
            return Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
#endif
#else
            return Input.GetMouseButtonUp(0);
#endif
        }

        public static Vector2 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
#if UNITY_EDITOR
            return Mouse.current.position.ReadValue();
#else
            return Touchscreen.current.primaryTouch.position.ReadValue();
#endif
#else
            return Input.mousePosition;
#endif
        }
    }
}

