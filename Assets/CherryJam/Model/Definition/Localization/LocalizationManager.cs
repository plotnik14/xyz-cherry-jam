﻿using System;
using System.Collections.Generic;
using CherryJam.Model.Data.Properties;
using UnityEngine;

namespace CherryJam.Model.Definition.Localization
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;

        private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;

        public string LocaleKey => _localeKey.Value;

        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            var localeDef = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = localeDef.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke();
        }

        public string Localize(string key)
        {
            var localizedValue = _localization.TryGetValue(key, out var value)
                ? value
                : $"%%%{key}%%%";
            return FormatNewLines(localizedValue);
        }

        private string FormatNewLines(string text)
        {
            return text.Replace("\\n", "\n");
        }

        public Sprite LocalizeImage(string imageTag)
        {
            var sprite = Resources.Load<Sprite>($"LocalizedImages/{_localeKey.Value}/{imageTag}");
            return sprite;
        }

        public void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}