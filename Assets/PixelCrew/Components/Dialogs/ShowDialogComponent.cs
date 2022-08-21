using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definition;
using PixelCrew.UI.Hud.Dialogs;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
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