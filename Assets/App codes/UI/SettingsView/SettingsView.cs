using Core;
using System;
using System.Linq;
using UI.Base;
using UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsView
{
    public class SettingsView : BaseView
    {
        [SerializeField] private Dropdown langDropdown;
        [SerializeField] private SettingsToggle autologinToggle;
        [SerializeField] private SettingsToggle anonDataToggle;
        private GUIRoot _guiroot;

        private void Start()
        {
            langDropdown.options = I2.Loc.LocalizationManager.GetAllLanguages().
                Select(x => new Dropdown.OptionData(Strings.GetString(x))).ToList();

            autologinToggle.SetState(UserPreferences.AutoLog);
            anonDataToggle.SetState(UserPreferences.AnonymousStat);

            langDropdown.value = I2.Loc.LocalizationManager.GetAllLanguages().
                IndexOf(I2.Loc.LocalizationManager.CurrentLanguage);
        }

        public void OnAutoLoginChange()
        {
            UserPreferences.AutoLog = autologinToggle.state;
        }

        public void OnAnyonymousDataChange()
        {
            UserPreferences.AnonymousStat = anonDataToggle.state;
        }

        public void OnLanguageChange()
        {
            UserPreferences.LanguageChanged = true; 
            I2.Loc.LocalizationManager.CurrentLanguage = 
                I2.Loc.LocalizationManager.GetAllLanguages()[langDropdown.value];
            UserPreferences.Language = I2.Loc.LocalizationManager.CurrentLanguage;

            langDropdown.options = I2.Loc.LocalizationManager.GetAllLanguages().
               Select(x => new Dropdown.OptionData(Strings.GetString(x))).ToList();
        }

        public override void OnMenuSelected()
        {

        }

        public override void OnMenuDeselected()
        {

        }

        public void OnLogout()
        {
            _guiroot.Logout();
        }

        internal void SetGuiRoot(GUIRoot gUIRoot)
        {
            this._guiroot = gUIRoot;
        }
    }
}