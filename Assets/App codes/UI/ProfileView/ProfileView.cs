using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI.Base;
using UI.Base.TeacherList;
using UI.SubjectFacultyChooser;
using UI.TeacherProfile;
using UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ProfileView
{
    public class ProfileView : Base.BaseView, ITeacherChangedEvents
    {
        [SerializeField] private SubjectList subList;
        [SerializeField] private RectTransform ratingsPanel;
        [SerializeField] private RectTransform addNewSubjectPanel;
        [SerializeField] private CommentsView.CommentView commentView;
        [SerializeField] private BasePopup commentViewBase;
        [SerializeField] private SkillRatingList _skills;
        [SerializeField] private ProfileInfos _infos;
        [SerializeField] private SubjectAdder _subAdder;

        User currentUser;

        private bool IsTeacher
        {
            get { return currentUser is Teacher; }
        }

        private Teacher teacher
        {
            get { return currentUser as Teacher; }
        }

        private Student student
        {
            get { return currentUser as Student; }
        }

        private void Awake()
        {
            EventBus.Instance.register<ITeacherChangedEvents>(gameObject);
        }

        private void Start()
        {
            EventBus.Instance.unregister(gameObject);
        }

        public void OnCheckoutRatings()
        {
            commentView.LoadComments((currentUser as Teacher).comments, true);
            commentViewBase.OnOpen();
        }

        public void OnNewSubject()
        {
            _subAdder._profileView = this;
            _subAdder.SetTeacher(teacher);
            _subAdder.OnOpen();
        }
        
        private void LoadDefaultImg()
        {
            Sprite sp = Resources.Load<Sprite>(Strings.Paths.DefaultProfPic);
            _infos.SetPicture(sp);
        }
        
        public override void OnMenuSelected()
        {
            currentUser = UserManager.Instance.CurrentUser;
            _infos.Initialize(currentUser, null);
            ratingsPanel.gameObject.SetActive(IsTeacher);
            addNewSubjectPanel.gameObject.SetActive(IsTeacher);
            subList.gameObject.SetActive(IsTeacher);
            _skills.gameObject.SetActive(IsTeacher);
            if (IsTeacher)
            {
                ReloadSubjects();
                _skills.PurifyList();
                _skills.Initialize(currentUser as Teacher, true);
            }
            else
            {

            }
            StorageManager.Instance.GetPictureFor(currentUser.id, ImageDownloaded, true);
        }

        private void ImageDownloaded(byte[] file, string id)
        {
            Texture2D img = new Texture2D(100, 100, TextureFormat.RGB24, false, false);
             img.LoadImage(file);

            if (img == null)
            {
                LoadDefaultImg();
            }
            else
            {
                Sprite s = Sprite.Create(img,
                    new Rect(0, 0, img.width, img.height),
                    new Vector2(0.5f, 0.5f), 100);
                _infos.SetPicture(s);
            }
            file = null;
        }

        public override void OnMenuDeselected()
        {
            commentView.OnClose();
            _subAdder.OnClose();
        }

        public void TeacherChanged(string id)
        {
            if(currentUser.id == id) subList.LoadList(teacher.subjects);
        }

        internal void ReloadSubjects()
        {
            subList.LoadList(teacher.subjects);
        }
    }
}
