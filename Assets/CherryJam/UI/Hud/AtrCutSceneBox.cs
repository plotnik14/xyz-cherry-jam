using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Hud
{
    public class AtrCutSceneBox : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Image Image => _image;
    }
}