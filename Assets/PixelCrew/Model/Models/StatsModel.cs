using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Player;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    public class StatsModel : IDisposable
    {
        private readonly PlayerData _data;
        public event Action OnChanged;
        public event Action<StatId> OnUpgraded; // ToDo replace with OnChanged. Just following the lector for now
        public ObservableProperty<StatId> InterfaceSelectedStat = new ObservableProperty<StatId>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public StatsModel(PlayerData data)
        {
            _data = data;
            _disposable.Retain(InterfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke()));
        }
        
        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void LevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;

            if (def.Levels.Length <= nextLevel) return;
            
            var price = def.Levels[nextLevel].Price;
            
            if (!_data.Inventory.HasEnough(price)) return;

            _data.Inventory.Remove(price.ItemId, price.Count);
            _data.Levels.LevelUp(id);
            
            OnChanged?.Invoke();
            OnUpgraded?.Invoke(id);
        }
        
        public float GetValue(StatId id, int level = -1)
        {
            return GetLevelDef(id, level).Value;
        }

        public StatLevelDef GetLevelDef(StatId id, int level = -1)
        {
            if (level == -1)
                level = GetCurrentLevel(id);
            
            var def = DefsFacade.I.Player.GetStat(id);

            return def.Levels.Length > level
                ? def.Levels[level]
                : default;
        }
        
        public int GetCurrentLevel(StatId id) => _data.Levels.GetLevel(id);

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}