using System.Collections;
using CherryJam.Components.LevelManagement.SpawnPoints;
using CherryJam.Creatures.Hero;
using CherryJam.Model;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.CutScenes.HubCutScenes
{
    public class HubStartCutScene : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private UnityEvent _beforeStart;
        [SerializeField] private UnityEvent _afterEnd;

        private Hero _hero;
        
        private void Start()
        {
            if (GameSession.Instance.NextSpawnPointType != SpawnPointType.Default) return;
            
            _hero = FindObjectOfType<Hero>();
            
            _beforeStart?.Invoke();
            StartCoroutine(PlayCutScene());
        }

        private IEnumerator PlayCutScene()
        {
            var playTime = _duration;

            while (playTime > 0)
            {
                _hero.SetDirection(Vector2.right);
                playTime -= Time.deltaTime;
                yield return null;
            }

            _hero.SetDirection(Vector2.zero);
            _afterEnd?.Invoke();
        }
    }
}