using CherryJam.Creatures.Hero;
using CherryJam.Model;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CherryJam.UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        [SerializeField] private InputEnableComponent _input;
        
        private float _defaultTimeScale;
        
        protected override void Start()
        {
            base.Start();
            
            _input.SetInput(false);
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
            
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
        
        public void OnCloseMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _input.SetInput(true);
                base.Close();
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}