using UnityEngine;

namespace CherryJam.Components.LevelManagement.SpawnPoints
{
    public class SpawnPointComponent : MonoBehaviour
    {
        [SerializeField] private SpawnPointType _type;

        public SpawnPointType Type => _type;
        public Vector3 Position => transform.position;
    }
    
    public enum SpawnPointType
    {
        Default,
        Left,
        Right,
        Up,
        Down
    }
}