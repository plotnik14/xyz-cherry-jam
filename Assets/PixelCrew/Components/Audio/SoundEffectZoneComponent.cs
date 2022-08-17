using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace PixelCrew.Components.Audio
{
    public class SoundEffectZoneComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;

        [SerializeField] private AudioMixerSnapshot _main;
        [SerializeField] private AudioMixerSnapshot _withEcho;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Check(other)) return;

            _withEcho.TransitionTo(0.1f);
            Debug.Log("Transition to Echo");
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (!Check(other)) return;

            _main.TransitionTo(0.1f);
            Debug.Log("Transition to Main");
        }

        private bool Check(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return false;
            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return false;

            return true;
        }
    }
}
