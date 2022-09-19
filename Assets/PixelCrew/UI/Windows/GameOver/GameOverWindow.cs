using PixelCrew.Model;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.GameOver
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