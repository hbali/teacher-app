using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RatingView
{
    class RatingViewSkillItem : BaseCompactListItem
    {
        [SerializeField] private SliderWithDisplay ratingSlider;
        [SerializeField] private Text skillDescription;

        public int Rating
        {
            get
            {
                return ratingSlider.Rating;
            }
        }

        public string SkillId
        {
            get; set;
        }

        public override void Initialize(params object[] parameters)
        {
            skillDescription.text = Strings.GetString(parameters[0] as string);
            SkillId = parameters[1] as string;
            SetContentSize();
        }

        private void SetContentSize()
        {
            float fullWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
            skillDescription.GetComponent<LayoutElement>().preferredHeight = 50;
            ratingSlider.GetComponent<LayoutElement>().preferredHeight = 20;
        }

        public override void OnClick()
        {

        }
    }
}
