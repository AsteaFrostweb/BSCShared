using System;
using System.Runtime.CompilerServices;

public static class Debugging
{
    public delegate void DebugDelegate(string message);
    public static event DebugDelegate OnLog;
    public static event DebugDelegate OnLogError;
    public static event DebugDelegate OnLogWarning;

    public static bool verbose = false;
    public static void VerboseLog(string category, string log) 
    {
        if (verbose)
            Log(category, log);
    }

    public static void Log(string category, string message)
    {
        Console.WriteLine($"[{category}] {message}");

        
  
        OnLog?.Invoke($"[{category}] {message}");
    }

    public static void LogWarning(string category, string message)
    {
        Console.WriteLine($"[WARN:{category}] {message}");

        OnLogWarning?.Invoke($"[WARN:{category}] {message}");
    }

    public static void LogError(string category, string message)
    {
        Console.WriteLine($"[ERROR:{category}] {message}");

        OnLogError?.Invoke($"[ERROR:{category}] {message}");
    }
}
