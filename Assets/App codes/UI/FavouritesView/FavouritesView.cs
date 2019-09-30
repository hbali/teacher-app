using Core;
using DataLayer.Model;
using System.Collections.Generic;
using System.Linq;
using UI.AdvancedSearch;
using UI.Base;
using UI.Base.TeacherList;
using UI.TeacherProfile;
using UnityEngine;

namespace UI.FavouritesView
{
    public class FavouritesView : BaseView, ITeacherChangedEvents
    {
        [SerializeField] private SearchView searchView;
        [SerializeField] private FavTeacherListView favTeacherListView;

        private void Awake()
        {
            EventBus.Instance.register<ITeacherChangedEvents>(gameObject);
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void OnAdvancedSearch()
        {
            searchView.gameObject.SetActive(true);
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
            favTeacherListView.Filter(matchedTeachers);
        }

        public override void OnMenuSelected()
        {
            if (_repo.IsInitialized && !favTeacherListView.listLoaded)
            {
                favTeacherListView.TeachersDownloaded(_repo.GetAllModelsOfType<Teacher>().
                    Where(x => UserManager.Instance.CurrentUser.favTeachers.Contains(x.id)).ToList(),
                    UserManager.Instance.CurrentUser);
            }
        }

        public override void OnMenuDeselected()
        {
            EventBus.Instance.post<ITeacherProfileEvents>((e, d) => e.CloseAll());
        }

        public void TeacherChanged(string id)
        {
            favTeacherListView.TeacherChanged(id);
        }
    }
}
