using CherryJam.Model;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CherryJam.UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        
        protected override void Start()
        {
            base.Start();
            
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;

            GameSession.Instance.IsInGameMenuOpened = true;
        }

        public override void Close()
        {
            base.Close();
            GameSession.Instance.IsInGameMenuOpened = false;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            
            var session = GameSession.Instance;
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}