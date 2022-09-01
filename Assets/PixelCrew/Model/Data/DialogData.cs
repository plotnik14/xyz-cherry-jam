using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public struct DialogData
    {
        [SerializeField] public Sentence[] Sentences;
        [SerializeField] private DialogType _type;
        
        public DialogType Type => _type;  
    }
    
    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _value;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Side _side;

        public string Value => _value;
        public Sprite Icon => _icon;
        public Side Side => _side;

        public Sentence(string value, Sprite icon, Side side)
        {
            _value = value;
            _icon = icon;
            _side = side;
        }
    }
        
    public enum Side
    {
        Left,
        Right
    }
        
    public enum DialogType
    {
        Simple,
        Personalized
    }
}