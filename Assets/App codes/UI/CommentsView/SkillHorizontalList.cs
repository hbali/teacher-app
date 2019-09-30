using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.CompactListView;
using UI.TeacherProfile;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CommentsView
{
    public class SkillHorizontalList : MonoBehaviour
    {
        [SerializeField] private Text skills;


        internal void Initialize(Dictionary<Skill, int> SkillRatings)
        {
        }
    }
}
