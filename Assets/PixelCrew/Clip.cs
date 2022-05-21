using UnityEngine;

namespace PixelCrew
{
    class Clip : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNextClip;
        [SerializeField] private Sprite[] _sprites;

        public Clip(string name, bool loop, bool allowNextClip, Sprite[] sprites)
        {
            _name = name;
            _loop = loop;
            _allowNextClip = allowNextClip;
            _sprites = sprites;
        }

        public string GetName()
        {
            return _name;
        }

        public Sprite[] GetSprites()
        {

            return _sprites;
        }

        public bool IsLooped()
        {
            return _loop;
        }

        public bool IsAllowNextClip()
        {
            return _allowNextClip;
        }

        public static Clip GetEmptyInstance()
        {
            return new Clip("empty", false, false, new Sprite[0]);
        }
    }
}
