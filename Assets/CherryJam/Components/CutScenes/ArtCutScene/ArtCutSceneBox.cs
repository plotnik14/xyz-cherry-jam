using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.Components.CutScenes.ArtCutScene
{
    public class ArtCutSceneBox : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Image Image => _image;
    }
}