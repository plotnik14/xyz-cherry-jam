using System;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Localization;
using PixelCrew.UI.Hud.Dialogs;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private bool _useLocalization = true;
        [SerializeField] private DialogData _boundDialog;
        [SerializeField] private DialogDef _externalDialog;

        private DialogBoxController _dialogBox;
        
        public void Show()
        {
            _dialogBox = FindDialogBoxController();
            _dialogBox.ShowDialog(Data);
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