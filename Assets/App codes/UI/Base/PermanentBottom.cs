using DataLayer.Database;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.FavouritesView;
using UI.SettingsView;
using UnityEngine;

namespace UI.Base
{
    public enum MenuView
    {
        Home,
        Favourites,
        Profile,
        Settings
    }
    public class PermanentBottom : MonoBehaviour, IPermanentBottom
    {
        [SerializeField] HomeView.HomeView homeView;
        [SerializeField] FavouritesView.FavouritesView favouritesView;
        [SerializeField] ProfileView.ProfileView profileView;
        [SerializeField] SettingsView.SettingsView settingsView;
        [SerializeField] private GlobalToggle homeToggle;

        [SerializeField] private MenuSlider _menuSlider;

        public IUIStack _uiStack { get; set; }

        public MenuView CurrentView { get; set; }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            CurrentView = MenuView.Home;
            _uiStack.ViewChanged(CurrentView);
        }

        /// <summary>
        /// Fires when the user clicks on a bottom menu
        /// </summary>
        /// <param name="newView">View to animate to</param>
        public void OnMenuClick(string newView)
        {
            GetView(CurrentView).OnMenuDeselected();
            CurrentView = EnumExtensions.Parse<MenuView>(newView);
            GetView(CurrentView).OnMenuSelected();
            _uiStack.ViewChanged(CurrentView);
            _menuSlider.SlideMenuTo((int)CurrentView);            
        }

        private BaseView GetView(MenuView currentView)
        {
            switch (currentView)
            {
                case MenuView.Favourites:
                    {
                        return favouritesView;
                    }
                case MenuView.Home:
                    {
                        return homeView;
                    }
                case MenuView.Profile:
                    {
                        return profileView;
                    }
                case MenuView.Settings:
                    {
                        return settingsView;
                    }
                default:
                    return homeView;
            }
        }

        internal void SetToDefault()
        {
            OnMenuClick(MenuView.Home.ToString());
            homeToggle.OnClick();
        }
    }

    public interface IPermanentBottom
    {
        MenuView CurrentView { get; set; }
    }
}

