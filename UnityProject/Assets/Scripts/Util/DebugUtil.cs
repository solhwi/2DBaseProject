using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtil
{
    public static void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }

    public static void LogError(object message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
    }
}
