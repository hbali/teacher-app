using System;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using System.Collections.Generic;
using UI.Utilities;
using UI.Base;
using DataLayer.Database;
using Core;
using System.Linq;

namespace UI.TeacherProfile
{
    public class ProfileSubjectsList : BaseCompactVerticalListView<ProfileSubjectListItem>
    {
        [SerializeField] SliderWithDisplay slider;
        
        private Teacher teacher;
        private Subject selectedSubject;

        protected override float ItemSize
        {
            get
            {
                return 30;
            }
        }

        public void Initialize(Teacher teacher)
        {
            PurifyList();
            this.teacher = teacher;
            foreach (KeyValuePair<Skill, float> ratings in teacher.skillRatings)
            {
                ProfileSubjectListItem item = CreateSingleItem("Prefabs/ProfileSubjectListItem");
                item.Initialize(ratings.Key, ratings.Value);
                item.SetDependency(this);
            }
        }

        public void Rate()
        {
        }

        public void PurifyList()
        {
            DestroyAllItems();
        }

        internal void SetSelectedSubject(Subject sub)
        {

        }
    }
}