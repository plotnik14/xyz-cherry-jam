using CherryJam.Model.Definition.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Localization
{
    [RequireComponent(typeof(Image))]
    public class LocalizeImage : MonoBehaviour
    {
        [SerializeField] private string _localizationTag;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }

        private void Localize()
        {
            var sprite = LocalizationManager.I.LocalizeImage(_localizationTag);
            _image.sprite = sprite;
        }

        private void OnLocaleChanged()
        {
            Localize();
        }

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;
        }
    }
}