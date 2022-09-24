using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CherryJam.Creatures.Mobs.Boss
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;
        
        [ColorUsage(true, true)]
        [SerializeField] private Color _color;

        [ContextMenu("SetColor")]
        public void SetColor()
        {
            foreach (var light in _lights)
            {
                light.color = _color;
            }
        }
    }
}