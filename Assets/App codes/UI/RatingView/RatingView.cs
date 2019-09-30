using Core;
using DataLayer.Database;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UI.CompactListView;
using UI.TeacherProfile;
using UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.RatingView
{
    class RatingView : BaseCompactVerticalListView<RatingViewSkillItem>
    {
        [SerializeField] private Dropdown subjectDropdown;
        [SerializeField] private InputField commentInput;

        private bool listInitialized;
        Teacher teacher;
        private IRepository _repo;

        protected override float ItemSize => 70;

        public void Initialize(Teacher t)
        {
            teacher = t;
            if (!listInitialized)
            {
                foreach (Skill s in teacher.skillRatings.Keys)
                {
                    CreateSingleItem("Prefabs/SkillRater").Initialize(s.Description, s.id);
                }
                listInitialized = true;
            }
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            for (int i = 0; i < teacher.subjects.Count; i++)
            {
                options.Add(new Dropdown.OptionData(teacher.subjects[i].Name));
            }
            subjectDropdown.options = options;
        }

        public void SetDependencies(IRepository repo)
        {
            this._repo = repo;
        }

        public void OnClose()
        {
            GetComponentInParent<BasePopup>().OnClose();
        }

        public void OnRate()
        {
            if (RatingHasError()) return;

            Subject subjectToRate = teacher.subjects[subjectDropdown.value];
            string ratings = "";

            for (int i = 0; i < itemList.Count; i++)
            {                
                ratings += i != itemList.Count - 1 ? itemList[i].Rating + ";" : itemList[i].Rating.ToString();
            }
            DbComment dbcomment = new DbComment()
            {
                id = Guid.NewGuid().ToString(),
                description = commentInput.text,
                subjectId = subjectToRate.id,
                studentId = Core.UserManager.Instance.CurrentUser.id,
                teacherId = teacher.id,
                skillRatings = ratings,
                timeStamp = DateTime.UtcNow.Ticks
            };
            FirebaseManager.Instance.PushToCloud<DbComment>(dbcomment, RateSuccess);
            _repo.AddDbModel<Comment>(dbcomment);
        }

        private bool RatingHasError()
        {
            if (string.IsNullOrEmpty(commentInput.text))
            {
                MessagePopupManager.Instance.
                                   SetUpperText(Strings.GetString(Strings.Rating.empty_text)).
                                   SetRightButtonText(Strings.GetString(Strings.UI.OK)).
                                   OnlyRightButton(true).EnablePopup();
                return true;
            }
            else if (teacher.id == Core.UserManager.Instance.CurrentUser.id)
            {
                MessagePopupManager.Instance.
                                  SetUpperText(Strings.GetString(Strings.Rating.rate_yourself)).
                                  SetRightButtonText(Strings.GetString(Strings.UI.OK)).
                                  OnlyRightButton(true).EnablePopup();
                return true;
            }
            else if (teacher.comments.Select(x => x.StudentId).Contains(Core.UserManager.Instance.CurrentUser.id))
            {
                MessagePopupManager.Instance.
                                  SetUpperText(Strings.GetString(Strings.Rating.rate_more_than_once)).
                                  SetRightButtonText(Strings.GetString(Strings.UI.OK)).
                                  OnlyRightButton(true).EnablePopup();
                return true;
            }
            return false;
        }

        private void RateSuccess(bool isSuccess)
        {
            if (isSuccess)
            {
                _repo.RefreshModel<Teacher>(teacher.id);
                EventBus.Instance.post<ITeacherChangedEvents>((e, d) => e.TeacherChanged(teacher.id));
            }
            string upperText = isSuccess ? Strings.GetString(Strings.Rating.rateSuccess) : Strings.GetString(Strings.Rating.rateUnsuccess);

            MessagePopupManager.Instance.
                                    SetUpperText(Strings.GetString(upperText)).
                                    SetRightButtonText(Strings.GetString(Strings.UI.OK)).
                                    SetRightButtonAction(OnClose).
                                    OnlyRightButton(true).EnablePopup();
        }
    }
}
