using Core;
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

namespace UI.ProfileView
{
    class SubjectAdder : BasePopup
    {
        [SerializeField] private SubjectController _subController;
        public ProfileView _profileView;
        private Teacher teacher;

        public void Initialize(SubjectController _subController)
        {
            this._subController = _subController;
        }

        public void SetTeacher(Teacher teacher)
        {
            this.teacher = teacher;
            _subController.gameObject.SetActive(true);
            _subController.LoadTeacherSubjects(teacher);
        }

        public void OnAddThem()
        {
            List<Subject> subjects = _subController.GetSubjectsFromInput();
            subjects.AddRange(CreateNewSubjectsWithFaculty(_subController.GetTotallyNewlyAddedSubjects()));
            subjects.AddRange(CreateNewSubjects(_subController.GetNewlyAddedSubjects()));
            subjects.AddRange(CreateNewSubjects(_subController.GetModifiedSubjects()));
            teacher.AddSubjects(subjects);
            FirebaseManager.Instance.PushToCloud<DbTeacher>(teacher.GetDbModel(), SuccessUpload);
        }

        private void SuccessUpload(bool success)
        {
            if(success)
            {
                string upperText = success ? Strings.GetString(Strings.Subjects.successSubjectsAdded) :
                    Strings.GetString(Strings.Subjects.failSubjectsAdded);

                string lowerText = success ? "" : Strings.GetString(Strings.Account.try_again);

                MessagePopupManager.Instance.
                                        SetUpperText(Strings.GetString(upperText)+
                                        Strings.GetString(lowerText)).
                                        SetRightButtonText(Strings.GetString(Strings.UI.OK)).
                                        SetRightButtonAction(OnClose).
                                        OnlyRightButton(true).EnablePopup();
            }
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
            foreach (KeyValuePair<string, string> kvp in newSubs)
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

        public override void OnClose()
        {
            base.OnClose();
            _profileView?.ReloadSubjects();
        }
    }
}
