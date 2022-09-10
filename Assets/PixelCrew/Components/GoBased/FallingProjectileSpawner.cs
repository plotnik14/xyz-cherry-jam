using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class FallingProjectileSpawner : MonoBehaviour, IProjectileSpawner
    {
        [Space][Header("Area Config")]
        [SerializeField] private float _width;

        [Space][Header("Spawn Config")]
        [SerializeField] private DirectionalProjectile _projectilePrefab;
        [SerializeField] private float _count;
        [SerializeField] private float _delay;
        
        [ContextMenu("Spawn")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }
        
        private IEnumerator SpawnProjectiles()
        {
            var areaPosition = transform.position;
            
            var positionX = areaPosition.x;
            var positionXDelta = _width / (_count - 1);
            
            for (var i = 0; i < _count; i++)
            {
                var spawnPosition = new Vector3(positionX, areaPosition.y, areaPosition.z);
                var instance = SpawnUtils.Spawn(_projectilePrefab.gameObject, spawnPosition);
                
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(Vector2.down);

                yield return new WaitForSeconds(_delay);
                
                positionX += positionXDelta;
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var areaPosition = transform.position;
            
            Handles.DrawLine(areaPosition, areaPosition + new Vector3(_width, 0, 0));
            
            var positionX = areaPosition.x;
            var positionXDelta = _width / (_count - 1);
            
            for (var i = 0; i < _count; i++)
            {
                var spawnPosition = new Vector3(positionX, areaPosition.y, areaPosition.z);
                var arrowVertex = spawnPosition + new Vector3(0, -0.5f, 0);
                
                Handles.DrawLine(spawnPosition, arrowVertex);
                Handles.DrawLine(arrowVertex + new Vector3(-0.1f, 0.2f, 0), arrowVertex);
                Handles.DrawLine(arrowVertex + new Vector3(0.1f, 0.2f, 0), arrowVertex);
                    
                positionX += positionXDelta;
            }
        }
#endif
    }
}