using System;
using CherryJam.UI.LevelsLoader;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level1");
            };
            
            Close();
        }

        public void OnLanguage()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }
        
        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();           
        }
    }
}
