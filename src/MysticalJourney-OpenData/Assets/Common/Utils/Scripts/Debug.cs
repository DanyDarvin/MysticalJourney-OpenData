using UnityEngine;

namespace Common.Utils
{
    public static class Debug
    {
        public static void DrawPointer(Vector3 worldPosition, float radius, float seconds)
        {
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.up, Color.cyan, seconds);
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.down, Color.cyan, seconds);
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.left, Color.cyan, seconds);
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.right, Color.cyan, seconds);
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.forward, Color.cyan, seconds);
            UnityEngine.Debug.DrawRay(worldPosition, radius * Vector3.back, Color.cyan, seconds);
        }
    }
}