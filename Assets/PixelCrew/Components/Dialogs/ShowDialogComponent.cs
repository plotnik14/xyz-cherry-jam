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
            if (_dialogBox == null)
            {
                _dialogBox = FindObjectOfType<DialogBoxController>();
            }
            
            _dialogBox.ShowDialog(Data);
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
            var localizedSentences = new List<string>();

            foreach (var sentenceKey in data.Sentences)
            {
                localizedSentences.Add(LocalizationManager.I.Localize(sentenceKey));
            }

            return new DialogData(localizedSentences.ToArray());
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}