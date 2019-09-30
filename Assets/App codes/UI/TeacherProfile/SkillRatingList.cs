using System;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using System.Collections.Generic;
using Core;

namespace UI.TeacherProfile
{
    public class SkillRatingList : BaseCompactVerticalListView<SkillRatingListItem>
    {
        internal void Initialize(Teacher teacher, bool includeAll)
        {
            if (includeAll)
            {
                var item = CreateSingleItem("Prefabs/ProfileSkillRating");
                item.Initialize(Strings.GetString(Strings.Rating.All), teacher.Rating);
                item.SetBold();
            }
            foreach (KeyValuePair<Skill, float> kvp in teacher.skillRatings)
            {
                CreateSingleItem("Prefabs/ProfileSkillRating").Initialize(kvp.Key.Name, kvp.Value);
            }
        }

        internal void PurifyList()
        {
            DestroyAllItems();
        }
    }
}