using UnityEngine;

namespace CherryJam.Components.Movement
{
    public class MovementToAltarComponent : MovementToTargetComponent
    {
        protected override void Start()
        {
            _destination = GameObject.FindWithTag("Altar").transform;
            base.Start();
        }
    }
}