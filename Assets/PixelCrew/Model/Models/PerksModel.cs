using System;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definition;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;
        
        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            
            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        public string Used => _data.Perks.Used.Value;

        public void Unlock(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            var isEnoughResources = _data.Inventory.HasEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(perkId);
                OnChanged?.Invoke();
            }
        }
        
        public void UsePerk(string perkId)
        {
            _data.Perks.Used.Value = perkId;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }
        
        public void Dispose()
        {
            _trash.Dispose();
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.HasEnough(def.Price);
        }

        public List<string> GetActivePerks()
        {
            return _data.Perks.GetUnlocked();
        }

        public bool IsSuperThrowSupported => IsUnlocked("super-throw");
        public bool IsDoubleJumpSupported => IsUnlocked("double-jump");
        public bool IsMagicShieldSupported => IsUnlocked("magic-shield");
        public bool IsFreezeEnemiesSupported => IsUnlocked("freezing-enemies");
    }
}