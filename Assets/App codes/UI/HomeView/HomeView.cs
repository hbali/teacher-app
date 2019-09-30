using Core;
using DataLayer.Database;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.AdvancedSearch;
using UI.Base;
using UI.Base.TeacherList;
using UI.TeacherProfile;
using UnityEngine;

namespace UI.HomeView
{
    class HomeView : BaseView, IRepositoryEvents, ITeacherChangedEvents
    {
        [SerializeField] private HomeTeacherListView teacherList;
        [SerializeField] private SearchView searchView;
        [SerializeField] private RectTransform noResultPanel;

        private void Awake()
        {
            EventBus.Instance.register<IRepositoryEvents>(gameObject);
            EventBus.Instance.register<ITeacherChangedEvents>(gameObject);
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void RepoInitialized()
        {
            teacherList.TeachersDownloaded(_repo.GetAllModelsOfType<Teacher>().ToList(), UserManager.Instance.CurrentUser);
        }
        

        public void OnAdvancedSearch()
        {
            searchView.OnOpen();
            if (!searchView.IsInitialized)
            {
                searchView.Initialize(FilterTeachers, _repo.GetAllModelsOfType<Teacher>());
                searchView.LoadFaculties(_repo.GetAllModelsOfType<Faculty>());
                searchView.LoadSubjects(_repo.GetAllModelsOfType<Subject>());
                searchView.IsInitialized = true;
            }
        }

        private void FilterTeachers(IEnumerable<Teacher> matchedTeachers)
        {
            teacherList.Filter(matchedTeachers);
            EnableNoResultPanel(!matchedTeachers.Any());
        }

        private void EnableNoResultPanel(bool enable)
        {
            noResultPanel.gameObject.SetActive(enable);
        }

        public override void OnMenuSelected()
        {
            if (_repo.IsInitialized && !teacherList.listLoaded) RepoInitialized();
        }

        public override void OnMenuDeselected()
        {
            EventBus.Instance.post<ITeacherProfileEvents>((e, d) => e.CloseAll());
            searchView.OnClose();
        }

        public void TeacherChanged(string id)
        {
            teacherList.TeacherChanged(id);
        }
    }
}
