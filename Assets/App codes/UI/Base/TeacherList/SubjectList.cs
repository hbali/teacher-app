using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.CompactListView;

namespace UI.Base.TeacherList
{
    public class SubjectList : BaseCompactVerticalListView<SubjectItem>
    {
        protected override float ItemSize
        {
            get
            {
                return 25;
            }
        }

        public void LoadList(IEnumerable<Subject> subs, bool isAutoComplete = false)
        {
            DestroyAllItems();
            foreach(Subject s in subs)
            {
                CreateSingleItem("Prefabs/SubjectItem").Initialize(s, isAutoComplete ? null : new Action<Subject>(SubjectOnClick));
            }
        }

        private void SubjectOnClick(Subject sub)
        {

        }

        public void PruneList(string searchterm)
        {

        }
    }
}
