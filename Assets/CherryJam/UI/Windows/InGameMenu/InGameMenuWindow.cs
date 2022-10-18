using CherryJam.Creatures.Hero;
using CherryJam.Model;
using CherryJam.UI.Windows.Settings;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CherryJam.UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        [SerializeField] private HeroInputEnableComponent _heroInput;
        
        private PlayerInput _uiInput;
        
        protected override void Start()
        {
            _uiInput = GetComponent<PlayerInput>();
            
            base.Start();
            
            _heroInput.SetInput(false);
            TimeUtils.StopTime();
            
        }
        
        public void OnShowSettings()
        {
            SetUiInputActive(false);
            var window = WindowUtils.CreateWindow("UI/SettingsWindow");
            var settings = window.GetComponent<SettingsWindow>();
            settings.SetRefToInGameMenu(this);
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
                _heroInput.SetInput(true);
                base.Close();
            }
        }

        public void SetUiInputActive(bool isActive)
        {
            _uiInput.enabled = isActive;
        }
        
        private void OnDestroy()
        {
            TimeUtils.ResumeTime();
        }
    }
}