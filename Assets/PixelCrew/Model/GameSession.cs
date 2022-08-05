using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        
        public PlayerData Data => _data;

        private PlayerData _save;

        public void Awake()
        {
            LoadHud();

            if (SessionExists())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                Save();
            }
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
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
