using CherryJam.Model.Definition.Localization;

namespace CherryJam.UI.Windows.LocalizationWindow
{
    public class LocalizationWindow : AnimatedWindow
    {
        public void UpdateLocale(string localeId)
        {
            LocalizationManager.I.SetLocale(localeId);
        }
    }
}