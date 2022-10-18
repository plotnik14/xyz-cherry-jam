using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CherryJam.UI.LevelsLoader
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _loadingDelay;

        private static readonly int Enabled = Animator.StringToHash("Enabled");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            InitLoader();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive);
        }

        public void LoadLevel(string sceneName)
        {
            _animator.SetBool(Enabled, true);
            StartCoroutine(StartAnimation(sceneName));
        }

        private IEnumerator StartAnimation(string sceneName)
        {
            yield return RunLoadingDelay(_loadingDelay);
            var loading = SceneManager.LoadSceneAsync(sceneName);
            
            while (!loading.isDone)
                yield return null;

            _animator.SetBool(Enabled, false);
        }

        private IEnumerator RunLoadingDelay(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}