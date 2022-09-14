using System;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    public class PermanentlyDestructibleObject : MonoBehaviour
    {
        [SerializeField] private string _id;

        private GameSession _session;
        
        [ContextMenu("GenerateNewId")]
        private void GenerateNewId()
        {
            _id = Guid.NewGuid().ToString();
        }

        public void MarkAsDestroyed()
        {
            _session.MarkObjectAsDestroyed(_id);
        }

        private bool HasBeenDestroyed()
        {
            return _session.ObjectHasBeenDestroyed(_id);
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(_id))
            {
                Debug.LogWarning("Found PermanentlyDestructibleObject with empty id");
            }
            
            _session = GameSession.Instance;
            
            if (HasBeenDestroyed())
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            MarkAsDestroyed();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }
        }
#endif
        
    }
}