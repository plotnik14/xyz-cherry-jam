using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private GameSession _session;

        public void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void Exit()
        {
            _session.SaveProgress();
            SceneManager.LoadScene(_sceneName);
        }

    }
}
