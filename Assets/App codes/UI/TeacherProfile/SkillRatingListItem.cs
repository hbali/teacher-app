using Core;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TeacherProfile
{
    public class SkillRatingListItem : BaseCompactListItem
    {
        [SerializeField] private Text skillName;
        [SerializeField] private Text ratingText;

        public override void Initialize(params object[] parameters)
        {
            skillName.text = Strings.GetString(parameters[0] as string);
            float rounded = float.Parse(parameters[1].ToString()) * 100;
            rounded = Mathf.Round(rounded) / 100;
            ratingText.text = rounded == 0 ? "N/A" : rounded.ToString();
        }

        public override void OnClick()
        {

        }

        public void SetBold()
        {
            ratingText.font = Resources.Load<Font>("Fonts/Nexa Bold");
            skillName.font = Resources.Load<Font>("Fonts/Nexa Bold");

        }
    }
}