using System;
using CherryJam.Model;
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
        
        public void OnShowLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }

        public void OnStartGame()
        {
            LoadLevel("Level0__CutScene1");
        }
        
        public void LoadLevel(string sceneName)
        {
            CleanExistingSession();
            
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel(sceneName);
            };
            
            Close();
        }

        private void CleanExistingSession()
        {
            if (GameSession.Instance != null)
                Destroy(GameSession.Instance.gameObject);
        }

        public void OnCredsGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level6_Credits");
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
