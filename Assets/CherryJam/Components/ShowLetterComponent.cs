using CherryJam.Model.Definition.Localization;
using CherryJam.UI.Hud.Letter;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components
{
    public class ShowLetterComponent : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private UnityEvent _onComplete;

        public void Show()
        {
            var letterBox = GameObject.FindWithTag("LetterBox");
            var letterController = letterBox.GetComponent<LetterController>();
            var localizedText = LocalizationManager.I.Localize(_text);
            letterController.Show(localizedText, _onComplete);
        }
    }
}