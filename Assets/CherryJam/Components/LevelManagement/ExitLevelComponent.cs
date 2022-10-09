using CherryJam.Components.LevelManagement.SpawnPoints;
using CherryJam.Model;
using CherryJam.UI.LevelsLoader;
using UnityEngine;

namespace CherryJam.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private SpawnPointType _pointType;

        public void Exit()
        {
            var session = GameSession.Instance;
            session.NextSpawnPointType = _pointType;
            
            session.Save();
            
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}