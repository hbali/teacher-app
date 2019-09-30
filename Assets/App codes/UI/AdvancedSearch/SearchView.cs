using Core;
using DataLayer.Model;
using Extensions;
using Occult.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.AdvancedSearch
{
    class SearchView : BasePopup
    {
        [SerializeField] private Dropdown facList;
        [SerializeField] private Slider ratingSlider;
        [SerializeField] private GlobalToggle femaleToggle;
        [SerializeField] private GlobalToggle maleToggle;
        [SerializeField] private InputField subjectInput;

        private Action<IEnumerable<Teacher>> searchAction;
        private List<Teacher> matchedTeachers;
        private IEnumerable<Teacher> allTeachers;
        private Faculty[] faculties;
        private Subject[] subjects;

        public bool IsInitialized { get; set; }        

        public void LoadSubjects(IEnumerable<Subject> subjects, bool set = true)
        {
            this.subjects = subjects.ToArray();
        }

        public void LoadFaculties(IEnumerable<Faculty> faculties)
        {
            /*facList.SetZeroDisplayString(Strings.GetString(Strings.UI.universities));
            facList.SetMultipleDisplayString(Strings.GetString(Strings.UI.universities));*/

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            faculties = faculties.OrderBy(x => x.Name);
            foreach (Faculty f in faculties)
            {
                if (options.Where(x => x.text == f.Name).Count() == 0)
                    options.Add(new Dropdown.OptionData(f.Name));
            }
            facList.options = options;
            this.faculties = faculties.ToArray();
        }

        public void OnSearch()
        {
            matchedTeachers = new List<Teacher>();
            IEnumerable<Subject> selectedSubjects = GetSelectedSubsFromInput(subjects, subjectInput.text);
            IEnumerable<Faculty> selectedFaculties = new Faculty[] { faculties[facList.value] };

            bool bothGender = maleToggle.State && femaleToggle.State;
            bool isMale = maleToggle.State && !femaleToggle.State;
            bool isFemale = !maleToggle.State && femaleToggle.State;

            foreach (Teacher t in allTeachers)
            {
                if ((t.Rating >= ratingSlider.value || t.Rating == 0)
                    && ((selectedSubjects.Count() == 0 || t.subjects.Select(x => x.Name).HasSameElement(selectedSubjects.Select(x => x.Name)))
                    && (selectedFaculties.Count() == 0 || t.Faculties.HasSameElement(selectedFaculties))))
                {
                    if (bothGender)
                        matchedTeachers.Add(t);
                    else if (t.isMale && isMale) matchedTeachers.Add(t);
                    else if (!t.isMale && isFemale) matchedTeachers.Add(t);
                }
            }

            searchAction.Invoke(matchedTeachers.OrderByDescending(x => x.Rating));
            OnClose();
        }

        private IEnumerable<Subject> GetSelectedSubsFromInput(Subject[] items, string searchword)
        {
            return searchword.Length > 0 ? items.Where(x => x.Name.ToLower().Contains(searchword.ToLower())) :
                items;
        }
        
        private IEnumerable<T> GetSelectedItemsFromDropdown<T>(MultiSelectDropdown subList, T[] items)
        {
            List<T> ret = new List<T>();
            foreach (int i in subList.value)
            {
                ret.Add(items[i - 1]);
            }
            return ret;
        }
        
        public override void OnOpen()
        {
            base.OnOpen();
        }
        public override void OnClose()
        {
            base.OnClose();
        }

        internal void Initialize(Action<IEnumerable<Teacher>> filterTeachers, IEnumerable<Teacher> teachers)
        {
            this.searchAction = filterTeachers;
            allTeachers = teachers;
        }
    }
}
