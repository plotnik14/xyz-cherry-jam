using UnityEngine;

namespace CherryJam.Components.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _sfx;
        [SerializeField] private AudioSource _music;
        
        public static AudioManager Instance { get; private set; }

        public AudioSource Sfx => _sfx;
        public AudioSource Music => _music;

        public void Awake()
        {
            var existingManager = GetExistingManager();
            if (existingManager != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }
        
        private AudioManager GetExistingManager()
        {
            var managers = FindObjectsOfType<AudioManager>();

            foreach (var manager in managers)
            {
                if (manager != this)
                    return manager;
            }

            return null;
        }
    }
}