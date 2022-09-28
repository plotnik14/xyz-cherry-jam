using System.Collections;
using CherryJam.Model.Data;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space] [SerializeField] private float _textSpeed = 0.09f;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        [Space] [SerializeField] protected DialogContent _content;
        [Space] [SerializeField] protected GameObject _dialogCameraEffects;
        
        private DialogData _data;
        private int _currentSentenceIndex;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private UnityEvent _callback;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        protected Sentence CurrentSentence => _data.Sentences[_currentSentenceIndex];

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }

        public void ShowDialog(DialogData data)
        {
            _data = data;
            _currentSentenceIndex = 0;
            CurrentContent.Text.text = string.Empty;
            
            _dialogCameraEffects.SetActive(true);
            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        public void ShowDialog(DialogData data, UnityEvent callback)
        {
            _callback = callback;
            ShowDialog(data);
        }

        protected virtual DialogContent CurrentContent => _content;

        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty;
            var sentence = CurrentSentence;
            //CurrentContent.TrySetIcon(sentence.Icon);
            
            foreach (var letter in sentence.Value)
            {
                CurrentContent.Text.text += letter;
                // _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentenceIndex].Value;
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
            {
                StopCoroutine(_typingRoutine);
            }
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentenceIndex++;

            var isDialogCompleted = _currentSentenceIndex >= _data.Sentences.Length;
            if (isDialogCompleted)
            {
                HideDialogBox();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
            _dialogCameraEffects.SetActive(false);
        }

        private void OnCloseAnimationComplete()
        {
            _callback?.Invoke();
        }
        
        
        // [Space][Header("Testing")]
        // [SerializeField] private DialogData _testData;
        //
        // public void DialogTest()
        // {
        //     ShowDialog(_testData);
        // }
    }
}