using System;
using System.Collections;
using CherryJam.Model.Data;
using CherryJam.Model.Definition;
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
            while (_dialogBox == null)
            {
                var dialogBoxObj = GameObject.FindWithTag(dialogBoxTag);

                if (dialogBoxObj != null)
                {
                    _dialogBox = dialogBoxObj.GetComponent<DialogBoxController>();
                    _dialogBox.ShowDialog(Data, _onComplete);
                    _hasBeenShown = true;
                }
                else
                    yield return null;
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
                        return _boundDialog;
                    case Mode.External:
                        return _externalDialog.Data;
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