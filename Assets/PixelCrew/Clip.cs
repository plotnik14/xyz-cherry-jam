using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    class Clip : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;

        public Sprite[] GetSprites()
        {

            return _sprites;
        }
    }
}
