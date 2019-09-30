using DataLayer.Model;
using System;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base.TeacherList
{
    public class SubjectItem : BaseCompactListItem
    {
        [SerializeField] private Text subText;

        public Subject sub;
        private Action<Subject> onClickCallback;

        public override void Initialize(params object[] parameters)
        {
            this.sub = parameters[0] as Subject;
            subText.text = sub.Name;
            if (parameters.Length > 1) onClickCallback = parameters[1] as Action<Subject>;
        }

        public override void OnClick()
        {
            if (onClickCallback != null) onClickCallback.Invoke(sub);
        }
    }
}