using UnityEngine;

public static class InputHelper
{
    public static Vector2 GetPointerPosition()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
                return Input.GetTouch(0).position;
            else
                return Vector2.zero;
#else
            return Vector2.zero; 
#endif
    }

    public static bool IsPointerDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButton(0);
#elif UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0;
#else
            return false;
#endif
    }
}