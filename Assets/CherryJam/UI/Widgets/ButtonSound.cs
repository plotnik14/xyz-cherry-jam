using CherryJam.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CherryJam.UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private AudioClip _pointerClick;
        [SerializeField] private AudioClip _pointerEnter;
        
        private AudioSource _source;

        public void OnPointerClick(PointerEventData eventData)
        {
            PlaySound(_pointerClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySound(_pointerEnter);
        }

        private void PlaySound(AudioClip sound)
        {
            if (_source == null)
            {
                _source = AudioUtils.FindSfxSource();
            }

            _source.PlayOneShot(sound);
        }
    }
}
