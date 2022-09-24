using CherryJam.Model;
using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Player;
using CherryJam.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Windows.PlayerStats
{
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private ProgressBarWidget _progress;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private StatDef _data;
        
        private void Start()
        {
            _session = GameSession.Instance;
            UpdateView();
        }
        
        public void SetData(StatDef data, int index)
        {
            _data = data;
            if (_session != null)
                UpdateView();
        }
        
        private void UpdateView()
        {
            var statsModel = _session.StatsModel;
            
            _icon.sprite = _data.Icon;
            _name.text = _data.Name;
            // _name.text = LocalizationManager.I.Localize(_data.Name);

            var currentValue = statsModel.GetValue(_data.Id);
            _currentValue.text = currentValue.ToString("0");

            var currentLevel = statsModel.GetCurrentLevel(_data.Id);
            var nextLevel = currentLevel + 1;
            var newValue = statsModel.GetValue(_data.Id, nextLevel);
            _increaseValue.text = $"+{newValue - currentValue}";
            _increaseValue.gameObject.SetActive(newValue > 0);

            var maxLevel = DefsFacade.I.Player.GetStat(_data.Id).Levels.Length - 1;
            var progress = (float) currentLevel / maxLevel;
            _progress.SetProgress(progress);
            
            _selector.SetActive(statsModel.InterfaceSelectedStat.Value == _data.Id);
        }

        public void OnSelect()
        {
            _session.StatsModel.InterfaceSelectedStat.Value = _data.Id;
        }
    }
}