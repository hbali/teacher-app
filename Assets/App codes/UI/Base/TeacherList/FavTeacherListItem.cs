using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;
using UI.TeacherProfile;
using Core;

namespace UI.Base.TeacherList
{
    public class FavTeacherListItem : TeacherListItem
    {
        private FavTeacherListView favListView;
        public override void Initialize(params object[] parameters)
        {
            favListView = parameters[1] as FavTeacherListView;
            base.Initialize(parameters);
        }

        public override void OnAddToFavourites()
        {
            base.OnAddToFavourites();
            favListView.UnloadElement(this);
            EventBus.Instance.post<IHomeTeacherListViewEvents>((e, d) => e.FavouritesChanged(Teacher, false));
        }

        internal void LoadPicture(Sprite pic)
        {
            teacherImage.sprite = pic;
        }
    }
}
