using System;
using System.Collections.Generic;
using CherryJam.Creatures.Hero;
using CherryJam.Model.Data;
using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Localization;
using CherryJam.UI.Hud.Dialogs;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private bool _useLocalization = true;
        [SerializeField] private bool _oneTimeDialog = true;
        [SerializeField] private DialogData _boundDialog;
        [SerializeField] private DialogDef _externalDialog;
        [SerializeField] private UnityEvent _onComplete;

        private DialogBoxController _dialogBox;
        private bool _hasBeenShown = false;
        
        public void Show()
        {
            if (_oneTimeDialog && _hasBeenShown)
            {
                _onComplete?.Invoke();
                return;
            }
            
            _dialogBox = FindDialogBoxController();
            _dialogBox.ShowDialog(Data, _onComplete);
            _hasBeenShown = true;
        }

        private DialogBoxController FindDialogBoxController()
        {
            if (_dialogBox != null) return _dialogBox;
            
            switch (Data.Type)
            {
                case DialogType.Simple:
                    return GameObject.FindWithTag("SimpleDialog").GetComponent<DialogBoxController>();
                case DialogType.Personalized:
                    return GameObject.FindWithTag("PersonalizedDialog").GetComponent<DialogBoxController>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Show(DialogDef dialogDef)
        {
            _externalDialog = dialogDef;
            Show();
        }
        
        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _useLocalization 
                            ? LocalizeData(_boundDialog) 
                            : _boundDialog;
                    case Mode.External:
                        return _useLocalization 
                            ? LocalizeData(_externalDialog.Data) 
                            : _externalDialog.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private DialogData LocalizeData(DialogData data)
        {
            var localizedData = data;
            var localizedSentences = new List<Sentence>();

            foreach (var sentence in localizedData.Sentences)
            {
                var localizedSentence = new Sentence(
                    LocalizationManager.I.Localize(sentence.Value),
                    sentence.Icon, 
                    sentence.Side
                );      
                
                localizedSentences.Add(localizedSentence);
            }

            localizedData.Sentences = localizedSentences.ToArray();
            return localizedData;
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}