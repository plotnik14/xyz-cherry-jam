using System.Collections;
using PixelCrew.Utils;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Components.GoBased
{
    public class RandomSpawner : MonoBehaviour
    {
        [SerializeField] private bool _usePool = true;
        
        [Header("Spawn bound:")]
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private float _sectorRotation;

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;
        [SerializeField] private float _itemPerBurst = 2;


        private Coroutine _routine;

        private void OnDisable()
        {
            TryStopRoutine();
        } 

        private void OnDestroy()
        {
            TryStopRoutine();
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        public void StartDrop(GameObject[] items)
        {
            TryStopRoutine();

            _routine = StartCoroutine(StartSpawn(items));
        }

        private IEnumerator StartSpawn(GameObject[] particles)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                Spawn(particles[i]);
                i++;

                for (var j = 0; j < _itemPerBurst && i < particles.Length; j++)
                {
                    Spawn(particles[i]);
                    i++;
                }

                yield return new WaitForSeconds(_waitTime);
            }
        }

        private void Spawn(GameObject particle)
        {
            var instance = _usePool 
                ? Pool.Instance.Get(particle, transform.position) 
                : SpawnUtils.Spawn(particle, transform.position);
            
            
            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);
            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            var rightBound = GetUnitOnCircle(middleAngleDelta);
            UnityEditor.Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            UnityEditor.Handles.DrawLine(position, position + leftBound);
            UnityEditor.Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, 1);

            UnityEditor.Handles.color = new Color(1f, 1f, 1f, 0.1f);
            UnityEditor.Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, 1);
        }
#endif
        
        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }
    }
}