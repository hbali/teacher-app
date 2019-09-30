using Core;
using Core.Validation;
using DataLayer.Database;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UI.SubjectFacultyChooser;
using UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoginView
{
    class RegisterView : MonoBehaviour, ILoginEvents
    {
        [SerializeField] private RectTransform facebookInfo;
        [SerializeField] private RectTransform connectWithFbButton;
        [SerializeField] private RectTransform registerInfos;
        [SerializeField] private RectTransform registerButton;
        [SerializeField] private Button registerButtonButton;
        [SerializeField] private Toggle termsAndConditionstoggle;
        [SerializeField] private GlobalToggle teacherToggle;

        [SerializeField] private RectTransform mainPanel;

        [SerializeField] private FacebookInfoHandler fbHandler;
        [SerializeField] private RectTransform toggleHolder;
        [SerializeField] private Dropdown genderDropdown;
        [SerializeField] private InputField emailInput;
        [SerializeField] private SubjectController _subjectController;
        [SerializeField] private AutoCompleteList _autoCompleteList;
        [SerializeField] private InputField messengerInput;
        [SerializeField] private RectTransform messengerHelp;

        Login _login;
        private string fbToken;
        private LoginView _loginView;
        private IRepository _repo;
        private bool isTeacher;

        internal void SetDependencies(IRepository repo)
        {
            this._repo = repo;
        }

        private void Awake()
        {
            EventBus.Instance.register<ILoginEvents>(gameObject);
            _subjectController.Initialize(_autoCompleteList);
            _autoCompleteList.Initialize(_subjectController);
        }

        private void Start()
        {
            genderDropdown.options = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData(Strings.GetString(Strings.Account.male)),
                new Dropdown.OptionData(Strings.GetString(Strings.Account.female)),
            };
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void EnablePanel(Login login, LoginView loginView)
        {
            _loginView = loginView;
            _login = login;
        }


        #region user interactions
        public void OnConnectWithFacebook()
        {
            _login.InitiateFacebookLogin("public_profile", "email");
        }


        public void OnRegister()
        {
            if (isTeacher)
            {
                if (EmailValidator.Validate(emailInput.text))
                {
                    FirebaseManager.Instance.EntityExistsWithFbId<DbTeacher>(_login.FbId, RegisterTeacher);
                }
                else
                {
                    MessagePopupManager.Instance.
                        SetUpperText(Strings.GetString(Strings.Account.error_email_format)).
                        SetLeftButtonText(Strings.GetString(Strings.UI.OK)).
                        OnlyRightButton(true).EnablePopup();
                }
            }
            else
            {
                FirebaseManager.Instance.EntityExistsWithFbId<DbStudent>(_login.FbId, RegisterStudent);
            }
        }

        private void RegisterStudent(bool doesExist)
        {
            if (!doesExist)
            {
                UserPreferences.CurrentUserIsTeacher = false;
                CreateStudent();
            }
            else
            {
                ShowAlreadyExistsPopup();
            }
        }

        private void ShowAlreadyExistsPopup()
        {
            MessagePopupManager.Instance.
                                    SetUpperText(Strings.GetString(Strings.Account.account_already_exists)).
                                    SetLeftButtonText(Strings.GetString(Strings.UI.OK)).
                                    OnlyRightButton(true).EnablePopup();
        }

        private void RegisterTeacher(bool doesExist)
        {
            if (!doesExist)
            {
                UserPreferences.CurrentUserIsTeacher = true;
                RegisterWithSubjects();
            }
            else
            {
                ShowAlreadyExistsPopup();
            }
        }

        private void ShowRegisterSuccessPopup(bool success)
        {
            if (success)
            {
                MessagePopupManager.Instance.
                                    SetUpperText(Strings.GetString(Strings.Account.success_register +
                                    Strings.GetString(Strings.Account.click_button_to_login))).
                                    SetRightButtonAction(OnOkButton).
                                    SetRightButtonText(Strings.GetString(Strings.Account.login)).OnlyRightButton(true).EnablePopup();
            }
            else
            {
                MessagePopupManager.Instance.
                                    SetUpperText(Strings.GetString(Strings.Account.error01) + 
                                    Strings.GetString(Strings.Account.try_again)).
                                    SetRightButtonText(Strings.GetString(Strings.UI.OK)).OnlyRightButton(true).EnablePopup();
            }
        }

        private void OnOkButton()
        {
            this.gameObject.SetActive(false);
            _loginView.AutoLogin();
        }

        private void CreateStudent()
        {
            Student stud = new Student()
            {
                id = Guid.NewGuid().ToString(),
                FavTeachers = new List<string>(),
                facebookId = _login.FbId,
                name = _login.FbName,
                email = emailInput.text,
                messagingToken = UserPreferences.MessagingToken
            };
            UserPreferences.CurrentUserId = stud.id;
            FirebaseManager.Instance.PushToCloud<DbStudent>(stud.GetDbModel(), ShowRegisterSuccessPopup);
            FirebaseManager.Instance.PushToFbConnectionTable(stud.GetDbModel() as DbUser);
            StorageManager.Instance.UploadImage(fbHandler.GetPicture().EncodeToPNG(), stud.id, false);
            _repo.AddModel(stud);
        }

        private void RegisterWithSubjects()
        {
            List<Subject> subjects = _subjectController.GetSubjectsFromInput();
            subjects.AddRange(CreateNewSubjectsWithFaculty(_subjectController.GetTotallyNewlyAddedSubjects()));
            subjects.AddRange(CreateNewSubjects(_subjectController.GetNewlyAddedSubjects()));
            subjects.AddRange(CreateNewSubjects(_subjectController.GetModifiedSubjects()));
            if (subjects.Count == 0)
            {
                ShowNoSubjectsPopup();
                return;
            }

            string msgId = messengerInput.text;
            if (msgId.Contains("/"))
            {
                msgId = msgId.Substring(msgId.IndexOf("/"));
            }
            Teacher teacher = new Teacher()
            {
                id = Guid.NewGuid().ToString(),
                name = fbHandler.FbName.text,
                subjects = subjects,
                fbId = _login.FbId,
                email = emailInput.text,
                isMale = genderDropdown.value == 0,
                messengerId = msgId,
                messagingToken = UserPreferences.MessagingToken
            };
            UserPreferences.CurrentUserId = teacher.id;
            DbTeacher dbModel = teacher.GetDbModel() as DbTeacher;
            FirebaseManager.Instance.PushToCloud<DbTeacher>(dbModel, ShowRegisterSuccessPopup);
            FirebaseManager.Instance.PushToFbConnectionTable(dbModel);
            StorageManager.Instance.UploadImage(fbHandler.GetPicture().EncodeToPNG(), teacher.id, false);
            _repo.AddModel(teacher);
        }

        private void ShowNoSubjectsPopup()
        {
            MessagePopupManager.Instance.
                                    SetUpperText(Strings.GetString(Strings.Account.no_subjects_reg)).
                                    SetLeftButtonText(Strings.GetString(Strings.UI.OK)).
                                    OnlyRightButton(true).EnablePopup();
        }

        private IEnumerable<Subject> CreateNewSubjects(List<KeyValuePair<string, Faculty>> list)
        {
            List<Subject> subs = new List<Subject>();
            foreach (KeyValuePair<string, Faculty> kvp in list)
            {
                Subject sub = new Subject()
                {
                    id = Guid.NewGuid().ToString(),
                    Faculty = kvp.Value,
                    Name = kvp.Key
                };
                subs.Add(sub);
                FirebaseManager.Instance.PushToCloud<DbSubject>(sub.GetDbModel());
            }
            return subs;
        }

        private IEnumerable<Subject> CreateNewSubjectsWithFaculty(List<KeyValuePair<string, string>> newSubs)
        {
            List<Subject> subs = new List<Subject>();
            foreach(KeyValuePair<string, string> kvp in newSubs)
            {
                Faculty fac = new Faculty()
                {
                    id = Guid.NewGuid().ToString(),
                    Name = kvp.Value
                };
                Subject sub = new Subject()
                {
                    id = Guid.NewGuid().ToString(),
                    Faculty = fac,
                    Name = kvp.Key
                };
                subs.Add(sub);
                FirebaseManager.Instance.PushToCloud<DbFaculty>(fac.GetDbModel());
                FirebaseManager.Instance.PushToCloud<DbSubject>(sub.GetDbModel());
            }
            return subs;
        }

        public void OnMessengerQuestion()
        {
            messengerHelp.gameObject.SetActive(true);
        }

        public void OnCloseMessenngerHelp()
        {
            messengerHelp.gameObject.SetActive(false);
        }
            
        public void OnBack()
        {
            mainPanel.gameObject.SetActive(false);
        }

        public void OnOpenMessenger()
        {
            Application.OpenURL("https://m.me/");
        }

        public void OnTeacherToggleChanged()
        {
            isTeacher = teacherToggle.State;
            EnableTeacherRegistrationPanels(isTeacher);
        }

        public void OnAgreeToTermsAndConditions()
        {
            registerButtonButton.interactable = termsAndConditionstoggle.isOn;
        }

        public void OnGenderDropdownValueChanged(int value)
        {

        }
#endregion

        private void EnableTeacherRegistrationPanels(bool enable)
        {
            registerInfos.gameObject.SetActive(enable);
        }
        
        private void SuccessAppLogin()
        {

        }

        private void RequestFacebookDatas()
        {
            FacebookUtilities.Instance.GetProfilePicture(PictureDownloadDone, FailedFbRequest);
            FacebookUtilities.Instance.GetProfileData(ProfileDatDownloadDone, FailedFbRequest);
        }

        private void ProfileDatDownloadDone(IDictionary<string, object> data)
        {
            UserPreferences.FbName = data["name"].ToString();
            fbHandler.SetName(data["name"].ToString());
            fbToken = data["id"].ToString();
            emailInput.text = data.ContainsKey("email") ? data["email"].ToString() : "";
            toggleHolder.gameObject.SetActive(true);
            int genderValue;
            switch(data["gender"].ToString())
            {
                case "male":
                    {
                        genderValue = 0;
                        break;
                    }
                case "female":
                    {
                        genderValue = 1;
                        break;
                    }
                default:
                    {
                        genderValue = 2;
                        break;
                    }
            }
            genderDropdown.value = genderValue;
        }

        private void PictureDownloadDone(Texture2D img)
        {
            fbHandler.SetPicture(img);
        }

        private void FailedFbRequest(string obj)
        {
            
        }

        #region facebook callbacks
        public void FailedFacebookLogin()
        {

        }

        public void SuccessFacebookLogin()
        {
            facebookInfo.gameObject.SetActive(true);
            connectWithFbButton.gameObject.SetActive(false);
            registerButton.gameObject.SetActive(true);
            termsAndConditionstoggle.gameObject.SetActive(true);
            RequestFacebookDatas();
        }
#endregion
    }
}
