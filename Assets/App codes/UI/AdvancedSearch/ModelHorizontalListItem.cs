using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine.UI;
using UnityEngine;

namespace UI.AdvancedSearch
{
    class ModelHorizontalListItem : BaseCompactListItem
    {
        [SerializeField] private RectTransform selectedPanel;
        [SerializeField] private Text title;

        private Action<BaseModel, bool> onClickAction;
        private BaseModel model;

        public override void Initialize(params object[] parameters)
        {
            model = parameters[0] as BaseModel;
            if (model is Faculty) LoadAsFaculty();
            else if (model is Subject) LoadAsSubject();
        }

        private void LoadAsSubject()
        {
            title.text = (model as Subject).Name;
        }

        private void LoadAsFaculty()
        {
            title.text = (model as Faculty).Name;
        }

        public override void OnClick()
        {
            selectedPanel.gameObject.SetActive(!selectedPanel.gameObject.activeSelf);
            onClickAction.Invoke(model, selectedPanel.gameObject.activeSelf);
        }

        internal void SetOnClickAction(Action<BaseModel, bool> onClickAction)
        {
            this.onClickAction = onClickAction;
        }
    }
}
