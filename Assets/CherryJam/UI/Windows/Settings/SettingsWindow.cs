using CherryJam.Model.Data;
using CherryJam.UI.Widgets;
using CherryJam.UI.Windows.InGameMenu;
using UnityEngine;

namespace CherryJam.UI.Windows.Settings
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

        private InGameMenuWindow _refToInGameMenuWindow;
        
        protected override void Start()
        {
            base.Start();
            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
        }

        public override void Close()
        {
            base.Close();
            
            if (_refToInGameMenuWindow == null) return;
            
            _refToInGameMenuWindow.SetUiInputActive(true);
        }

        public void SetRefToInGameMenu(InGameMenuWindow menu)
        {
            _refToInGameMenuWindow = menu;
        }
    }
}
