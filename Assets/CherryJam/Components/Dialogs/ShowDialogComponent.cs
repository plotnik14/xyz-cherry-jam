﻿using System;
using System.Collections;
using System.Collections.Generic;
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
            
            UpdateDialogBoxController();
        }

        private void UpdateDialogBoxController()
        {
            StartCoroutine(FindAndUpdateDialogBox("PersonalizedDialog"));
        }

        private IEnumerator FindAndUpdateDialogBox(string dialogBoxTag)
        {
            if (_dialogBox == null)
            {
                GameObject dialogBoxObj = null;
                while (dialogBoxObj == null)
                {
                    dialogBoxObj = GameObject.FindWithTag(dialogBoxTag);

                    if (dialogBoxObj == null)
                        yield return null;
                }
                
                _dialogBox = dialogBoxObj.GetComponent<DialogBoxController>();
            }
            
            _dialogBox.ShowDialog(Data, _onComplete);
            _hasBeenShown = true;
        }

        public void Show(DialogDef dialogDef)
        {
            _externalDialog = dialogDef;
            Show();
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

        public enum Mode
        {
            Bound,
            External
        }
    }
}