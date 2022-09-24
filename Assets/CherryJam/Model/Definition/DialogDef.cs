﻿using CherryJam.Model.Data;
using UnityEngine;

namespace CherryJam.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private DialogData _data;

        public DialogData Data => _data;
    }
}