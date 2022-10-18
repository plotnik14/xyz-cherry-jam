using System.Collections;
using UnityEngine;

namespace CherryJam.UI.Windows.AuthorsWindow
{
    public class AuthorsWindowController : MonoBehaviour
    {
        private GameObject _canvasObj;
        private void Awake()
        {
            StartCoroutine(ShowAuthorsWindow());
        }

        private IEnumerator ShowAuthorsWindow()
        {
            var window = Resources.Load<GameObject>("UI/AuthorsWindow");
            
            while (_canvasObj == null)
            {
                _canvasObj = GameObject.FindWithTag("MainUICanvas");
                yield return null;
            }
            
            Instantiate(window, _canvasObj.GetComponent<Canvas>().transform);
        }
    }
}