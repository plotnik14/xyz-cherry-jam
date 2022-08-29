using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Repositories;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class ActivePerksContainer : MonoBehaviour
    {
        [SerializeField] private ActivePerkWidget _prefab;
        
        private DataGroup<PerkDef, ActivePerkWidget> _dataGroup;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private GameSession _session;

        public void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _dataGroup = new DataGroup<PerkDef, ActivePerkWidget>(_prefab, transform);
            _disposable.Retain(_session.PerksModel.Subscribe(OnPerksChanged));
        }
        
        private void OnPerksChanged()
        {
            var perkId = _session.PerksModel.Used;
            if (string.IsNullOrEmpty(perkId)) return;
            
            var perkDef = DefsFacade.I.Perks.Get(perkId);
            _dataGroup.SetData(new List<PerkDef> {perkDef});
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}