using CherryJam.Model;
using CherryJam.UI.LevelsLoader;
using UnityEngine;

namespace CherryJam.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            var session = GameSession.Instance;
            session.Save();
            
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}