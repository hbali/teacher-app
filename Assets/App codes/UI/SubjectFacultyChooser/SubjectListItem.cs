using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SubjectFacultyChooser
{
    public class SubjectListItem : BaseCompactListItem
    {
        [SerializeField] private Dropdown facDropdown;
        [SerializeField] private Text subjectInput;
        [SerializeField] private Text facultyText;
        [SerializeField] private RectTransform plus;
        [SerializeField] private RectTransform minus;
        [SerializeField] private RectTransform inputdisabled;
        [SerializeField] private RectTransform placeholder;

        private ISubjectController _subjectController;

        private Subject subject;
        private Faculty faculty;

        private List<Faculty> allFaculties;
        private List<Subject> allSubjects;
        public bool isadded;

        public Subject Subject
        {
            get
            {
                return subject;
            }
        }

        public Faculty Faculty
        {
            get
            {
                return faculty;
            }
        }

        public override void Initialize(params object[] parameters)
        {
            subject = parameters[0] is Subject ? parameters[0] as Subject : subject;
            if (parameters[0] is List<Faculty>)
            {
                this.allFaculties = parameters[0] as List<Faculty>;
                LoadDropdown();
            }
            else if(parameters[0] is Faculty)
            {
                this.allFaculties = new List<Faculty>() { parameters[0] as Faculty};
                LoadDropdown();
            }
            SetTexts();
        }

        internal void DisableEdit()
        {
            facDropdown.interactable = false;
            inputdisabled.gameObject.SetActive(true);
            DisabledPlaceholder();
        }

        private void DisabledPlaceholder()
        {
            placeholder.gameObject.SetActive(false);
        }

        internal void AlreadyAdded(bool added)
        {
            isadded = added;
            plus.gameObject.SetActive(!added);
            minus.gameObject.SetActive(added);
        }

        private void LoadDropdown()
        {
            facDropdown.options = new List<Dropdown.OptionData>(allFaculties.Select(x => new Dropdown.OptionData(x.Name)));
            faculty = allFaculties[0];
        }

        public KeyValuePair<string, string> GetSubFaculty
        {
            get
            {
                return new KeyValuePair<string, string>(subjectInput.text, facultyText.text);
            }
        }

        private void SetTexts()
        {
            subjectInput.text = subject != null ? subject.Name : subjectInput.text;
            facultyText.text = faculty != null ? faculty.Name : facultyText.text;
            if(subject != null) DisabledPlaceholder();
        }

        public void OnAdd()
        {
            _subjectController.AddNewItem();
        }

        public void OnDelete()
        {
            _subjectController.DeleteItem(this);
        }

        public void OnDropdownChange()
        {
            faculty = allFaculties[facDropdown.value];
            facultyText.text = faculty.Name;
        }

        internal void SetDependency(ISubjectController subjectController)
        {
            this._subjectController = subjectController;
        }

        public void SetSubjectInputText(string txt)
        {
            subjectInput.text = txt;
            DisabledPlaceholder();
        }

        public override void OnClick()
        {
            _subjectController.SetSelectedItem(this);
            _subjectController.EnableAutoComplete(true);
        }
    }
}
