using DataLayer.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI.Utilities;
using UI.CompactListView;
using System.Linq;
using Core;
using DataLayer.Database;

namespace UI.SubjectFacultyChooser
{
    public class SubjectController : BaseCompactVerticalListView<SubjectListItem>, ISubjectController
    {
        public AutoCompleteList autoComplete;
        private IRepository _repo;
        private SubjectListItem selectedItem;

        [SerializeField] private bool isOnTeacherProfile;
        User user;
        private Teacher teacher;

        protected override float ItemSize
        {
            get
            {
                return 40;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (!isOnTeacherProfile)
            {
                NewSubject();
            }
        }

        public void LoadTeacherSubjects(Teacher teacher)
        {
            Purify();
            this.teacher = teacher;
            foreach(Subject s in teacher.subjects)
            {
                SubjectListItem item = CreateSingleItem("Prefabs/Subject");
                item.SetDependency(this);
                item.Initialize(s);
                item.Initialize(s.Faculty);
                item.DisableEdit();
                item.AlreadyAdded(true);
            }
            NewSubject();
        }

        private void NewSubject()
        {
            SubjectListItem item = CreateSingleItem("Prefabs/Subject");
            item.SetDependency(this);
            item.Initialize(_repo.GetAllModelsOfType<Faculty>().ToList());
            SetSelectedItem(item);
            item.AlreadyAdded(false);
        }

        public void RepoInitialized()
        {
            autoComplete.CreateList(_repo.GetAllModelsOfType<Subject>().ToList());
        }

        internal void SetDependencies(IRepository repo)
        {
            this._repo = repo;
        }

        internal void Initialize(AutoCompleteList autoComplete)
        {
            this.autoComplete = autoComplete;
            autoComplete.Initialize(this);
        }
        public void AddNewItem()
        {
            NewSubject();
        }

        public void DeleteItem(SubjectListItem subjectListItem)
        {
            teacher.RemoveSubject(subjectListItem.Subject);
            FirebaseManager.Instance.PushToCloud<DbTeacher>(teacher.GetDbModel());
            teacher.LoadDependentFields();
            DestroySingleItem(subjectListItem);
        }

        public void SetSelectedItem(SubjectListItem item)
        {
            this.selectedItem = item;
        }

        internal List<Subject> GetSubjectsFromInput()
        {
            List<Subject> subs = new List<Subject>();
            foreach(SubjectListItem item in itemList)
            {
                if (item.Subject != null && item.Faculty != null && !item.isadded
                    && _repo.ContainsSubjectWithFaculty(item.Subject.id, item.Faculty.id))
                {
                    subs.Add(item.Subject);
                }
            }
            return subs;
        }

        /// <summary>
        /// Totally newly added: neither the subject nor the faculty existed in the database
        /// </summary>
        /// <returns></returns>
        internal List<KeyValuePair<string, string>> GetTotallyNewlyAddedSubjects()
        {
            List<KeyValuePair<string, string>> newSubs = new List<KeyValuePair<string, string>>();
            foreach (SubjectListItem item in itemList)
            {
                if (item.Faculty == null && !item.isadded
                    && !string.IsNullOrEmpty(item.GetSubFaculty.Key) && !string.IsNullOrEmpty(item.GetSubFaculty.Value))
                {
                    newSubs.Add(item.GetSubFaculty);
                }
            }
            return newSubs;
        }

        /// <summary>
        /// Returns the subjects that existed but have new faculties
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, Faculty>> GetModifiedSubjects()
        {
            List<KeyValuePair<string, Faculty>> newSubs = new List<KeyValuePair<string, Faculty>>();
            foreach (SubjectListItem item in itemList)
            {
                if (item.Subject != null && item.Faculty != null && !item.isadded
                    && !_repo.ContainsSubjectWithFaculty(item.Subject.id, item.Faculty.id))
                {
                    newSubs.Add(new KeyValuePair<string, Faculty>(item.GetSubFaculty.Key, item.Faculty));
                }
            }
            return newSubs;
        }

        /// <summary>
        /// Newly added subjects: the subject did not but the faculty did exist in the database
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, Faculty>> GetNewlyAddedSubjects()
        {
            List<KeyValuePair<string, Faculty>> newSubs = new List<KeyValuePair<string, Faculty>>();
            foreach (SubjectListItem item in itemList)
            {
                if (item.Subject == null && item.Faculty != null && !item.isadded
                    && !string.IsNullOrEmpty(item.GetSubFaculty.Key))
                {
                    newSubs.Add(new KeyValuePair<string, Faculty>(item.GetSubFaculty.Key, item.Faculty));
                }
            }
            return newSubs;
        }

        public void InitializeSelectedItem(Subject model)
        {
             selectedItem.Initialize(model);
        }

        public void SetSelectedSubjectWithText(string sub)
        {
            selectedItem.SetSubjectInputText(sub);
        }

        public void Purify()
        {
            DestroyAllItems();
        }

        public void EnableAutoComplete(bool enable)
        {
            autoComplete.Initialize(this);
            autoComplete.EnablePanel(enable);
        }
    }

    internal interface ISubjectController
    {
        void AddNewItem();
        void DeleteItem(SubjectListItem subjectListItem);
        void SetSelectedItem(SubjectListItem item);
        void InitializeSelectedItem(Subject model);
        void EnableAutoComplete(bool enable);
        void SetSelectedSubjectWithText(string sub);
    }
}
