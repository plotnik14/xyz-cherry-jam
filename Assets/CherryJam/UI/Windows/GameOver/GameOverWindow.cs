using CherryJam.Model;
using UnityEngine.SceneManagement;

namespace CherryJam.UI.Windows.GameOver
{
    public class GameOverWindow : AnimatedWindow
    {
        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            
            var session = GameSession.Instance;
            Destroy(session.gameObject);
        }
    }
}