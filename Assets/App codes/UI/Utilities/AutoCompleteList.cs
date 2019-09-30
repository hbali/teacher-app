using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.CompactListView;
using UnityEngine;
using UI.SubjectFacultyChooser;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UI.Base.TeacherList;

namespace UI.Utilities
{
    public class AutoCompleteList : BaseCompactVerticalListView<SubjectItem>
    {
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] private InputField search;
 
        private ISubjectController _subjectController;

        private List<Subject> subjects;

        protected override float ItemSize
        {
            get
            {
                return 30;
            }
        }

        public void CreateList(List<Subject> items)
        {
            subjects = items;
            foreach (Subject s in subjects)
            {
                if (itemList == null || itemList.Where(x => x.sub.Name.ToLower() == s.Name.ToLower()).Count() == 0)
                    CreateSingleItem("Prefabs/SubjectItem").Initialize(s, new Action<Subject>(SubjectOnClick));
            }
        }

        private void SubjectOnClick(Subject sub)
        {
            _subjectController.InitializeSelectedItem(sub);
            EnablePanel(false);
        }

        public void OnDone()
        {
            string txt = search.text.Length > 1 ? search.text[0].ToString().ToUpper() + search.text.Substring(1).ToLower() : search.text;
            Subject foundSub = subjects.Where(x => x.Name.ToLower() == txt.ToLower()).FirstOrDefault();
            if (foundSub != null)
            {
                _subjectController.InitializeSelectedItem(foundSub);
            }
            else _subjectController.SetSelectedSubjectWithText(txt);
            EnablePanel(false);
        }

        public void OnInputChange()
        {
            foreach(SubjectItem si in itemList)
            {
                if (search.text.Length >= 3)
                {
                    if (si.sub.Name.ToLower().Contains(search.text.ToLower()))
                        TurnOnSingleElement(si);
                    else TurnOffSingleElement(si);
                }
                else TurnOnSingleElement(si);
            }            
        }

        public void EnablePanel(bool on)
        {
            gameObject.SetActive(on);
        }

        internal void Initialize(ISubjectController subjectController)
        {
            this._subjectController = subjectController;
        }
    }
}
