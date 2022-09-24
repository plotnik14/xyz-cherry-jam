﻿using CherryJam.Model;
using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Repositories;
using CherryJam.UI.Widgets;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Windows.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _perksContainer;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;
        
        protected override void Start()
        {
            base.Start();
            _session = GameSession.Instance;

            _trash.Retain(_session.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));
            
            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_perksContainer);
            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.PerksModel.InterfaceSelection.Value;
            // _useButton.gameObject.SetActive(_session.PerksModel.IsUnlocked(selected));
            // _useButton.interactable = _session.PerksModel.Used != selected;
            
            _buyButton.gameObject.SetActive(!_session.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = _session.PerksModel.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _info.text = def.Info;
            // _info.text = LocalizationManager.I.Localize(def.Info);
        }
        
        private void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.UsePerk(selected);
        }

        private void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Unlock(selected);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}