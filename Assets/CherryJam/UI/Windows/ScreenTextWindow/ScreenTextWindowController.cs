using System.Collections;
using UnityEngine;

namespace CherryJam.UI.Windows.ScreenTextWindow
{
    public class ScreenTextWindowController : MonoBehaviour
    {
        [SerializeField] private string _windowToLoad;
        
        private GameObject _canvasObj;
        private void Awake()
        {
            StartCoroutine(LoadWindow());
        }

        private IEnumerator LoadWindow()
        {
            var window = Resources.Load<GameObject>(_windowToLoad);
            
            while (_canvasObj == null)
            {
                _canvasObj = GameObject.FindWithTag("MainUICanvas");
                yield return null;
            }
            
            Instantiate(window, _canvasObj.GetComponent<Canvas>().transform);
        }
    }
}