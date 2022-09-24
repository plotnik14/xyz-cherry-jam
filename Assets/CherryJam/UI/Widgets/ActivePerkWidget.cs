using CherryJam.Creatures.Hero;
using CherryJam.Model.Definition.Repositories;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Widgets
{
    public class ActivePerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _cooldownCounter;
        [SerializeField] private GameObject _isLocked;

        private Hero _hero;
        private PerkDef _data;
        private float _curretCooldown;
        private bool _isCooldownActive;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public void SetData(PerkDef data, int index)
        {
            _isCooldownActive = false;
            _data = data;

            _icon.sprite = _data.Icon;
            _cooldownCounter.text = string.Empty;
            _isLocked.SetActive(false);
            
            _hero = FindObjectOfType<Hero>();
            
            if (data.Cooldown > 0)
                _disposable.Retain(_hero.SubscribePerkUse(OnPerkUsed));
        }

        private void OnPerkUsed(string perkId)
        {
            if (perkId != _data.Id) return;

            _curretCooldown = _data.Cooldown;
            _cooldownCounter.text = _curretCooldown.ToString("0.0");
            _isCooldownActive = true;
            // _isLocked.SetActive(true);
        }
        
        private void Update()
        {
            if (!_isCooldownActive) return;

            _curretCooldown -= Time.deltaTime;

            if (_curretCooldown <= 0)
            {
                _isCooldownActive = false;
                // _isLocked.SetActive(true);
            }

            _cooldownCounter.text = _isCooldownActive 
                ? _curretCooldown.ToString("0.0") 
                : string.Empty;
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}