using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using Core;
using UnityEngine.EventSystems;

namespace UI.Base.TeacherList
{
    class HomeTeacherListView : TeacherListView<HomeTeacherListItem>, IHomeTeacherListViewEvents
    {
        protected override string PrefabPath
        {
            get
            {
                return "Prefabs/HomeTeacherListItem";
            }
        }

        protected override void Awake()
        {
            base.Awake();
            EventBus.Instance.register<IHomeTeacherListViewEvents>(gameObject);
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void FavouritesChanged(Teacher teacher, bool isFav)
        {
            itemList.Where(x => x.Teacher == teacher).FirstOrDefault().SetFavourite(isFav);
        }

        internal void TeacherChanged(string id)
        {
            HomeTeacherListItem item = itemList.Where(x => x.Teacher.id == id).FirstOrDefault();
            if(item != null) item.LoadTeacherData();
        }
    }

    internal interface IHomeTeacherListViewEvents : IEventSystemHandler
    {
        void FavouritesChanged(Teacher teacher, bool isFav);
    }
}
