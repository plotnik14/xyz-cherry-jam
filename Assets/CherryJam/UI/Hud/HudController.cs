using CherryJam.Model;
using CherryJam.Model.Definition;
using CherryJam.UI.Widgets;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _heathBar;

        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Hp.OnChanged += OnHealthChanged;

            OnHealthChanged(_session.Data.Hp.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHP;
            var value = (float)newValue / maxHealth;
            _heathBar.SetProgress(value);
        }

        public void OnSettings()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }
        
        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }
        
        public void OnShowPlayerStats()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow");
        }
    }
}
