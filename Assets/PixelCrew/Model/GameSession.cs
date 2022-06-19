using System;
using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        
        public PlayerData Data => _data;

        private PlayerData _dataOnLevelStart;

        public void Awake()
        {
            if (SessionExists())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                SaveProgress();
            }
        }

        public void SaveProgress()
        {
            _dataOnLevelStart = _data.GetCopy();
        }

        public void ResetToLevelStart()
        {
            _data = _dataOnLevelStart.GetCopy();
        }

        private bool SessionExists()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach (var session in sessions)
            {
                if (session != this)
                    return true;
            }

            return false;
        }
    }
}
