using CherryJam.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CherryJam.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = GameSession.Instance;
            session.LoadLastSave();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
