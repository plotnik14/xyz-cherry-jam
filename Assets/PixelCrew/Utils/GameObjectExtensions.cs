using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return layerMask == (layerMask | 1 << gameObject.layer);
        }
    }
}
