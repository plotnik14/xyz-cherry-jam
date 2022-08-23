using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definition.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {
        // en = https://docs.google.com/spreadsheets/d/e/2PACX-1vTpCebtBWNI-HEqCCrd7oZKl-PdgO0kxT2w0m5W9QjmnJL1xVheW25l-G7_nkX_C_4_Y7S8YSszxzZr/pub?gid=0&single=true&output=tsv
        // ru = https://docs.google.com/spreadsheets/d/e/2PACX-1vTpCebtBWNI-HEqCCrd7oZKl-PdgO0kxT2w0m5W9QjmnJL1xVheW25l-G7_nkX_C_4_Y7S8YSszxzZr/pub?gid=2143532475&single=true&output=tsv
        // es = https://docs.google.com/spreadsheets/d/e/2PACX-1vTpCebtBWNI-HEqCCrd7oZKl-PdgO0kxT2w0m5W9QjmnJL1xVheW25l-G7_nkX_C_4_Y7S8YSszxzZr/pub?gid=1487689210&single=true&output=tsv
        
        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;
        
        public Dictionary<string, string> GetData()
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in _localeItems)
            {
                dictionary.Add(item.Key, item.Value);
            }

            return dictionary;
        }

        [ContextMenu("Update locale")]
        public void UpdateLocale()
        {
            if (_request != null) return;

            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                var rows = _request.downloadHandler.text.Split('\n');
                _localeItems.Clear();
                
                foreach (var row in rows)
                {
                    AddLocaleItem(row);
                }
            }
        }

        private void AddLocaleItem(string row)
        {
            try
            {
                var parts = row.Split('\t');
                _localeItems.Add(new LocaleItem(){Key = parts[0], Value = parts[1]});
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't parse row:{row}. \n {e}");
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}