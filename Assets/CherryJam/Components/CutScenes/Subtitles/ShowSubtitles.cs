using System;
using System.Collections;
using System.Collections.Generic;
using CherryJam.Model.Definition.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.Components.CutScenes.Subtitles
{
    public class ShowSubtitles : MonoBehaviour
    {
        [SerializeField] public Text _text;
        [SerializeField] public List<SubtitleData> _subtitles;

        private void Start()
        {
            StartCoroutine(ShowSubtitlesCoroutine());
        }

        private IEnumerator ShowSubtitlesCoroutine()
        {
            foreach (var subtitleData in _subtitles)
            {
                var localizedText = LocalizationManager.I.Localize(subtitleData.Text);
                _text.text = localizedText;
                yield return new WaitForSeconds(subtitleData.Duration);
            }
        }
    }

    [Serializable]
    public struct SubtitleData
    {
        [SerializeField] private string _text;
        [SerializeField] private float _duration;

        public string Text => _text;
        public float Duration => _duration;
    }
}