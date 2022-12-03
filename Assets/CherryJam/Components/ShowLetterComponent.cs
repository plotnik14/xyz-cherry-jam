using CherryJam.UI.Hud.Letter;
using UnityEngine;

namespace CherryJam.Components
{
    public class ShowLetterComponent : MonoBehaviour
    {
        [SerializeField][TextArea] private string _text;

        public void Show()
        {
            var letterBox = GameObject.FindWithTag("LetterBox");
            var letterController = letterBox.GetComponent<LetterController>();

            letterController.Show(_text);
        }
    }
}