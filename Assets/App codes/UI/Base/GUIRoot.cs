using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.HomeView;
using UI.LoginView;
using UI.SubjectFacultyChooser;
using UnityEngine;

namespace UI.Base
{
    public class GUIRoot : MonoBehaviour, IRepositoryEvents
    {
        [SerializeField] SubjectController _subjectController;
        [SerializeField] SubjectController _subjectController2;

        [SerializeField] HomeView.HomeView _homeView;
        [SerializeField] ProfileView.ProfileView _profileView;
        [SerializeField] RegisterView _registerView;
        [SerializeField] RatingView.RatingView _ratingView;
        [SerializeField] SettingsView.SettingsView _settingsView;
        [SerializeField] MenuSlider _slider;

        [SerializeField] FavouritesView.FavouritesView _favView;
        [SerializeField] LoginView.LoginView _loginView;
        [SerializeField] PermanentBottom permanentbottom;
        [SerializeField] private MenuSlider _menuSlider;
        [SerializeField] private RectTransform screenParents;

        [SerializeField] private RectTransform autoCompleteParent;
        [SerializeField] private RectTransform subjectControllerParent;

        IRepository _repo;
        private void Awake()
        {
            IUIStack uiStack = gameObject.AddComponent<UIStack>();
            uiStack.SetDependencies(_slider, permanentbottom);
            BasePopup.SetDependencies(uiStack);
            permanentbottom._uiStack = uiStack;

            Application.targetFrameRate = 45;
            EventBus.Instance.register<IRepositoryEvents>(gameObject);
            StorageManager instance = StorageManager.Instance;

            _repo = new Repository();
            BaseModel.SetDependencies(_repo);

            _subjectController.SetDependencies(_repo);
            _subjectController2.SetDependencies(_repo);
            _favView.SetDependencies(_repo);
            _homeView.SetDependencies(_repo);
            _registerView.SetDependencies(_repo);
            _ratingView.SetDependencies(_repo);
            _loginView.SetDependencies(this);
            _settingsView.SetGuiRoot(this);
            UserManager.Instance.SetRepo(_repo);
            _repo.Initialize();
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        private void Start()
        {
            if (!UserPreferences.LanguageChanged)
            {
                I2.Loc.LocalizationManager.CurrentLanguage = Application.systemLanguage == SystemLanguage.Hungarian ?
                    Strings.Languages.Hungarian :
                    Strings.Languages.English;
            }
            else
            {
                I2.Loc.LocalizationManager.CurrentLanguage = UserPreferences.Language;
            }
        }    

        internal void LoadHomeView()
        {
            permanentbottom.gameObject.SetActive(true);
        }

        public void Logout()
        {
            _loginView.gameObject.SetActive(true);
            permanentbottom.SetToDefault();
            permanentbottom.gameObject.SetActive(false);
            _subjectController.Purify();
        }

        public void RepoInitialized()
        {
            _subjectController.RepoInitialized();
            _subjectController2.RepoInitialized();
        }
    }
}
