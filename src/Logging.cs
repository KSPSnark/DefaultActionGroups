using System;
using UnityEngine;

namespace DefaultActionGroups
{
    /// <summary>
    /// Simple wrapper around debug logging so that we have a consistent output.
    /// </summary>
    static class Logging
    {
        private static readonly string MODULE_PREFIX = "[DefaultActionGroups] ";

        public static void Log(object message)
        {
            Debug.Log(MODULE_PREFIX + message);
        }

        public static void Warn(object message)
        {
            Debug.LogWarning(MODULE_PREFIX + message);
        }

        public static void Error(object message)
        {
            Debug.LogError(MODULE_PREFIX + message);
        }

        public static void Exception(Exception e)
        {
            Error("(" + e.GetType().Name + ") " + e.Message + ": " + e.StackTrace);
        }

        public static string ToString(Part part)
        {
            return part.partInfo.title;
        }
    }
}
