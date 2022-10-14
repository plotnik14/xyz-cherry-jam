using System.Collections;
using System.Collections.Generic;
using CherryJam.Creatures;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.CutScenes
{
    public class CreatureMovementCutScene : MonoBehaviour
    {
        [SerializeField] private Creature _creature;
        [SerializeField] private List<Transform> _points;
        [SerializeField] private UnityEvent _beforeStart;
        [SerializeField] private UnityEvent _afterEnd;
        
        private void Start()
        {
            _beforeStart?.Invoke();
            StartCoroutine(PlayCutScene());
        }

        private IEnumerator PlayCutScene()
        {
            foreach (var point in _points)
            {
                var direction = _creature.transform.CalculateDirectionToPoint(point);
                
                while (!_creature.transform.IsOnPoint(point))
                {
                    _creature.SetDirection(direction);
                    yield return null;
                }
            }

            _creature.SetDirection(Vector2.zero);
            _afterEnd?.Invoke();
        }
    }
}