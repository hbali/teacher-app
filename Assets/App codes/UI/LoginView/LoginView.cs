using Core;
using DataLayer.Database;
using DataLayer.Model;
using MovementEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UI.Utilities;
using UnityEngine;

namespace UI.LoginView
{
    class LoginView : MonoBehaviour, ILoginEvents
    {
        [SerializeField] private RectTransform registerPanel;
        [SerializeField] private RegisterView _registerView;
        [SerializeField] private RectTransform registerInfos;

        private GUIRoot guiRoot;

        private Login _login;

        private void Awake()
        {
            EventBus.Instance.register<ILoginEvents>(gameObject);
            _login = new Login();           
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        private void Start()
        {
            bool userExists = !string.IsNullOrEmpty(UserPreferences.CurrentUserId) && !string.IsNullOrEmpty(_login.FbId);
            if (UserPreferences.AutoLog && userExists)
            {
                AutoLogin();
            }
            else if (!userExists)
            {
                _login.InitializeAuth();
            }
        }

        internal void SetDependencies(GUIRoot gUIRoot)
        {
            this.guiRoot = gUIRoot;
        }

        public void LoginAsTeacher()
        {
            MessagePopupManager.Instance.StartLoader(Strings.GetString(Strings.Account.logging_in));
            Timing.RunCoroutine(LoginTeacherCoroutine());
        }

        private IEnumerator<float> LoginTeacherCoroutine()
        {
            UserPreferences.CurrentUserIsTeacher = true;
            if (!string.IsNullOrEmpty(_login.FbId))
            {
                FirebaseManager.Instance.GetEntityWithFbId<DbTeacher>(_login.FbId, TeacherFetched);
            }
            else
            {
                _login.InitiateFacebookLogin("public_profile", "email");
            }
            yield return Timing.WaitForSeconds(0.01f);
        }

        public void LoginAsStudent()
        {
            MessagePopupManager.Instance.StartLoader(Strings.GetString(Strings.Account.logging_in));
            Timing.RunCoroutine(LoginStudentCoroutine());
        }

        private IEnumerator<float> LoginStudentCoroutine()
        {
            UserPreferences.CurrentUserIsTeacher = false;
            if (!string.IsNullOrEmpty(_login.FbId))
            {
                FirebaseManager.Instance.GetEntityWithFbId<DbStudent>(_login.FbId, StudentFetched);
            }
            else
            {
                _login.InitiateFacebookLogin("public_profile", "email");
            }
            yield return Timing.WaitForSeconds(0.01f);
        }

        private void TeacherFetched(DbTeacher loggedIn)
        {
            if(loggedIn == null)
            {
                MessagePopupManager.Instance.
                        SetUpperText(Strings.GetString(Strings.Account.teacher_doesnt_exist)).
                        SetLeftButtonText(Strings.GetString(Strings.UI.OK)).
                        OnlyRightButton(true).EnablePopup();
                MessagePopupManager.Instance.StopLoader();

            }
            else
            {
                if ((string.IsNullOrEmpty(loggedIn.messagingToken) && !string.IsNullOrEmpty(UserPreferences.MessagingToken))
                    || loggedIn.messagingToken != UserPreferences.MessagingToken)
                {
                    loggedIn.messagingToken = UserPreferences.MessagingToken;
                    FirebaseManager.Instance.PushToCloud<DbTeacher>(loggedIn);
                }
                UserPreferences.CurrentUserId = loggedIn.id;
                LoadMainMenu();
            }
        }

        private void StudentFetched(DbStudent loggedIn)
        {
            if (loggedIn == null)
            {
                MessagePopupManager.Instance.
                        SetUpperText(Strings.GetString(Strings.Account.student_doesnt_exist)).
                        SetLeftButtonText(Strings.GetString(Strings.UI.OK)).
                        OnlyRightButton(true).EnablePopup();
                MessagePopupManager.Instance.StopLoader();
            }
            else
            {
                UserPreferences.CurrentUserId = loggedIn.id;
                LoadMainMenu();
                if ((string.IsNullOrEmpty(loggedIn.messagingToken) && !string.IsNullOrEmpty(UserPreferences.MessagingToken))
                             || loggedIn.messagingToken != UserPreferences.MessagingToken)
                {
                    loggedIn.messagingToken = UserPreferences.MessagingToken;
                    FirebaseManager.Instance.PushToCloud<DbStudent>(loggedIn);
                }
            }
        }

        public void AutoLogin()
        {
            if (UserPreferences.CurrentUserIsTeacher) LoginAsTeacher();
            else LoginAsStudent();
        }

        private void LoadMainMenu()
        {
            registerInfos.gameObject.SetActive(false);
            guiRoot.LoadHomeView();
            this.gameObject.SetActive(false);
            MessagePopupManager.Instance.StopLoader();
        }

        public void OnRegister()
        {
            _login.InitializeAuth();
            registerPanel.gameObject.SetActive(true);
            _registerView.EnablePanel(_login, this);
        }

        public void FailedFacebookLogin()
        {
        }

        public void SuccessFacebookLogin()
        {
            MessagePopupManager.Instance.StopLoader();
            if (!_registerView.gameObject.activeSelf)
            {
                AutoLogin();
            }
        }

        public void OnInfos()
        {
            registerInfos.gameObject.SetActive(!registerInfos.gameObject.activeSelf);
        }
    }
}
