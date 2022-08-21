using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private DialogData _data;

        public DialogData Data => _data;
    }
}