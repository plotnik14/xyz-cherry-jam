using PixelCrew.Components;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _platformCheck;
        [SerializeField] private LayerCheck _wallCheck;

        private Creature _creature;
        private float _xDirection;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
            _xDirection = 1;
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (!_platformCheck.IsTouchingLayer || _wallCheck.IsTouchingLayer)
                {
                    _xDirection = -_xDirection;
                }

                _creature.SetDirection(new Vector2(_xDirection, 0));

                yield return null;
            }
        }
    }
}
