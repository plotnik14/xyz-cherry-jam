using UnityEngine;

namespace CherryJam.Components.Movement
{
    public class MovementToAltarComponent : MovementToTargetComponent
    {
        protected void Awake()
        {
            _destination = GameObject.FindWithTag("Altar").transform;
        }
    }
}