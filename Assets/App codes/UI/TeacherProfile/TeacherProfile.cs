using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Base;
using UI.Base.TeacherList;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TeacherProfile
{
    public class TeacherProfile : BasePopup, ITeacherProfileEvents, ITeacherChangedEvents
    {
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] private SkillRatingList skillRatingList;
        [SerializeField] private RatingView.RatingView ratingView;
        [SerializeField] private CommentsView.CommentView commentView;
        [SerializeField] private SubjectList subList;
        [SerializeField] private BasePopup ratingViewBase;
        [SerializeField] private BasePopup commentViewBase;
        [SerializeField] private ProfileInfos profile;

        [SerializeField] RectTransform messengerPanel;
        [SerializeField] RectTransform emailPanel;

        private Teacher teacher;
        private bool isFav;
        private Sprite img;

        protected override void Awake()
        {
            base.Awake();
            EventBus.Instance.register<ITeacherProfileEvents>(gameObject);
            EventBus.Instance.register<ITeacherChangedEvents>(gameObject);
        }

        private void OnDestroy()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void LoadTeacher(Teacher teacher, Sprite teacherImg)
        {
            OnOpen();
            this.img = teacherImg;
            this.teacher = teacher;
            isFav = UserManager.Instance.CurrentUser.favTeachers.Contains(teacher.id);
            profile.Initialize(teacher, teacherImg, isFav);
            skillRatingList.PurifyList();
            skillRatingList.Initialize(teacher, false);
            emailPanel.gameObject.SetActive(string.IsNullOrEmpty(teacher.messengerId));
            messengerPanel.gameObject.SetActive(!string.IsNullOrEmpty(teacher.messengerId));
            subList.LoadList(teacher.subjects);
        }

        public override void OnClose()
        {
            skillRatingList.PurifyList();
            base.OnClose();
        }

        public void OnMessenger()
        {
            Application.OpenURL("https://m.me/" + teacher.messengerId);
        }

        private void SendEmail(string email, string subject, string body)
        {
            subject = MyEscapeURL(subject);
            body = MyEscapeURL(body);
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }
        private string MyEscapeURL(string url)
        {
            return WWW.EscapeURL(url).Replace("+", "%20");
        }

        public void OnEmail()
        {
            SendEmail(teacher.email, "", "");
        }

        public void TeacherChanged(string id)
        {
            if (teacher != null && this.teacher.id == id)
            {
                LoadTeacher(teacher, null);
            }
        }

        public void OnRate()
        {
            ratingView.Initialize(teacher);
            ratingViewBase.OnOpen();
        }

        public void OnCheckoutRatings()
        {
            commentView.LoadComments(teacher.comments, false);
            commentViewBase.OnOpen();
        }

        public void OnAddToFavourites()
        {
            isFav = !isFav;
            profile.SetFavourite(isFav);
            UserManager.Instance.TeacherAddedToFavourites(teacher, isFav);
            EventBus.Instance.post<IFavTeacherListViewEvents>((e, d) => e.FavouritesChanged(teacher, isFav, img));
            EventBus.Instance.post<IHomeTeacherListViewEvents>((e, d) => e.FavouritesChanged(teacher, isFav));

        }

        public void CloseAll()
        {
            commentView.OnClose();
            ratingView.OnClose();
            this.OnClose();
        }
    }

    internal interface ITeacherProfileEvents : IEventSystemHandler
    {
        void LoadTeacher(Teacher teacher, Sprite teacherImg);

        void CloseAll();
    }

    public interface ITeacherChangedEvents : IEventSystemHandler
    {
        void TeacherChanged(string id);

    }
}
