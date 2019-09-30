using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Base.TeacherList
{
    public class FavTeacherListView : TeacherListView<FavTeacherListItem>, IFavTeacherListViewEvents
    {
        protected override string PrefabPath
        {
            get
            {
                return "Prefabs/FavTeacherListItem";
            }
        }

        protected override void Awake()
        {
            base.Awake();
            EventBus.Instance.register<IFavTeacherListViewEvents>(gameObject);
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void FavouritesChanged(Teacher teacher, bool isFav, Sprite pic)
        {
            if (isFav)
            {
                if (itemList.Where(x => x.Teacher == teacher).Count() == 0)
                {
                    var item = CreateSingleItem(PrefabPath);
                    item.Initialize(teacher, this);
                    item.LoadPicture(pic);
                    item.SetFavourite(true);
                }
            }
            else UnloadElement(itemList.Where(x => x.Teacher == teacher).FirstOrDefault());
        }
        internal void TeacherChanged(string id)
        {
            FavTeacherListItem item = itemList.Where(x => x.Teacher.id == id).FirstOrDefault();
            if (item != null) item.LoadTeacherData();
        }
    }

    internal interface IFavTeacherListViewEvents : IEventSystemHandler
    {
        void FavouritesChanged(Teacher teacher, bool isFav, Sprite pic);
    }
}
