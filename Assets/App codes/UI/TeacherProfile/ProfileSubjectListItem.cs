using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TeacherProfile
{
    public class ProfileSubjectListItem : BaseCompactListItem
    {
        [SerializeField] private Text subjectName;
        [SerializeField] private Text rating;
        [SerializeField] private RectTransform selectedBackground;

        private ProfileSubjectsList listView;
        public Skill skill;
 
        public override void Initialize(params object[] parameters)
        {
            skill = parameters[0] as Skill;
            float rating = (float)parameters[1];

            subjectName.text = skill.Name;
            this.rating.text = rating.ToString(); 
        }

        internal void SetDependency(ProfileSubjectsList profileSubjectsList)
        {
            listView = profileSubjectsList;
        }

        public override void OnClick()
        {
        }

        public void SetSelected(bool set)
        {
            selectedBackground.gameObject.SetActive(set);
        }
    }
}
