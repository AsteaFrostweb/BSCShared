using System;

public static class Debugging
{
    public static void Log(string category, string message)
    {
        Console.WriteLine($"[{category}] {message}");

#if UNITY_2020_1_OR_NEWER
        UnityEngine.Debug.Log($"[{category}] {message}");
#endif
    }

    public static void LogWarning(string category, string message)
    {
        Console.WriteLine($"[WARN:{category}] {message}");

#if UNITY_2020_1_OR_NEWER
        UnityEngine.Debug.LogWarning($"[WARN:{category}] {message}");
#endif
    }

    public static void LogError(string category, string message)
    {
        Console.WriteLine($"[ERROR:{category}] {message}");

#if UNITY_2020_1_OR_NEWER
        UnityEngine.Debug.LogError($"[ERROR:{category}] {message}");
#endif
    }
}
