using System.Collections;
using CherryJam.Creatures.Weapons;
using CherryJam.Utils;
using CherryJam.Utils.ObjectPool;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class GroupProjectileSpawner : MonoBehaviour, IProjectileSpawner
    {
        [SerializeField] private bool _usePool = true;
        
        [Space][Header("Area Config")]
        [SerializeField] private float _width;

        [Space][Header("Spawn Config")]
        [SerializeField] private DirectionalProjectile _projectilePrefab;
        [SerializeField] private float _count;
        [SerializeField] private float _delay;
        [SerializeField] private Direction _direction;
        
        [ContextMenu("Spawn")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }
        
        private IEnumerator SpawnProjectiles()
        {
            var direction = _direction == Direction.Up ? Vector2.up : Vector2.down;
            var areaPosition = transform.position;
            
            var positionX = areaPosition.x;
            var positionXDelta = _width / (_count - 1);
            
            for (var i = 0; i < _count; i++)
            {
                var spawnPosition = new Vector3(positionX, areaPosition.y, areaPosition.z);
                
                var instance = _usePool 
                    ? Pool.Instance.Get(_projectilePrefab.gameObject, spawnPosition) 
                    : SpawnUtils.Spawn(_projectilePrefab.gameObject, spawnPosition);
                
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);

                yield return new WaitForSeconds(_delay);
                
                positionX += positionXDelta;
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var areaPosition = transform.position;
            
            UnityEditor.Handles.DrawLine(areaPosition, areaPosition + new Vector3(_width, 0, 0));
            
            var positionX = areaPosition.x;
            var positionXDelta = _width / (_count - 1);
            
            for (var i = 0; i < _count; i++)
            {
                var spawnPosition = new Vector3(positionX, areaPosition.y, areaPosition.z);

                if (_direction == Direction.Up)
                    DrawUpArrows(spawnPosition);
                else
                    DrawDownArrows(spawnPosition);
                    
                positionX += positionXDelta;
            }
        }

        private void DrawUpArrows(Vector3 spawnPosition)
        {
            var arrowVertex = spawnPosition + new Vector3(0, 0.5f, 0);
                
            UnityEditor.Handles.DrawLine(spawnPosition, arrowVertex);
            UnityEditor.Handles.DrawLine(arrowVertex + new Vector3(-0.1f, -0.2f, 0), arrowVertex);
            UnityEditor.Handles.DrawLine(arrowVertex + new Vector3(0.1f, -0.2f, 0), arrowVertex);
        }
        
        private void DrawDownArrows(Vector3 spawnPosition)
        {
            var arrowVertex = spawnPosition + new Vector3(0, -0.5f, 0);
                
            UnityEditor.Handles.DrawLine(spawnPosition, arrowVertex);
            UnityEditor.Handles.DrawLine(arrowVertex + new Vector3(-0.1f, 0.2f, 0), arrowVertex);
            UnityEditor.Handles.DrawLine(arrowVertex + new Vector3(0.1f, 0.2f, 0), arrowVertex);
        }
#endif
    }
    
    public enum Direction
    {
        Up,
        Down
    }
}