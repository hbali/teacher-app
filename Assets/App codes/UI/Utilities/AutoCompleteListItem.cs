using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine.UI;
using UnityEngine;

namespace UI.Utilities
{
    class AutoCompleteListItem : BaseCompactListItem
    {
        [SerializeField] private Text itemText;

        private BaseModel model;
        private Action<BaseModel> selectedAction;

        public override void Initialize(params object[] parameters)
        {
            this.model = parameters[0] as BaseModel;
            itemText.text = model is Faculty ? (model as Faculty).Name : (model as Subject).Name;
        }

        public override void OnClick()
        {
            if (model is Faculty)
            {
                selectedAction(model as Faculty);
            }
            else if (model is Subject)
            {
                selectedAction(model as Subject);
            }
        }

        internal void SetSelectedAction(Action<BaseModel> selectItem)
        {
            selectedAction = selectItem;
        }
    }
}
