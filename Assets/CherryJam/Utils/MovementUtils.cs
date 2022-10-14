using UnityEngine;

namespace CherryJam.Utils
{
    public static class MovementUtils
    {
        private const float Threshold = 1f;
        
        public static bool IsOnPoint(this Transform current, Transform destination)
        {
            return (destination.position - current.position).magnitude < Threshold;
        }
        
        public static Vector2 CalculateDirectionToPoint(this Transform current, Transform destination)
        {
            var direction = destination.position - current.position;
            return direction.normalized;
        }
    }
}