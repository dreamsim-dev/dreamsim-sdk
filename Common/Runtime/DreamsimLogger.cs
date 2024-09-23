using System;
using UnityEngine;

namespace Dreamsim
{
public static class DreamsimLogger
{
    public static void Log(object msg)
    {
        Debug.Log($"[Dreamsim] {msg}");
    }

    public static void LogWarning(object msg)
    {
        Debug.LogWarning($"[Dreamsim] {msg}");
    }

    public static void LogError(object msg)
    {
        Debug.LogError($"[Dreamsim] {msg}");
    }

    public static void LogException(Exception e)
    {
        Debug.LogException(e);
    }
}
}