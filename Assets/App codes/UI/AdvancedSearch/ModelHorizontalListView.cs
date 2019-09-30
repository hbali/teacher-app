using DataLayer.Model;
using System;
using System.Collections.Generic;
using UI.CompactListView;

namespace UI.AdvancedSearch
{
    internal class ModelHorizontalListView : BaseCompactHorizontalListView<ModelHorizontalListItem>
    {
        public void LoadModels(IEnumerable<BaseModel> models, Action<BaseModel, bool> onClickAction)
        {
            foreach (BaseModel m in models)
            {
                ModelHorizontalListItem item = CreateSingleItem("Prefabs/SearchView/SubFacItem");
                item.Initialize(m);
                item.SetOnClickAction(onClickAction);
            }
        }
    }
}