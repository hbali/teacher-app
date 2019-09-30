using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;
using UI.TeacherProfile;
using Core;
using System.Threading;

namespace UI.Base.TeacherList
{
    public class TeacherListItem : BaseCompactListItem
    {
        [SerializeField] protected Text teacherName;
        [SerializeField] protected Text rating;
        [SerializeField] protected Image teacherImage;
        [SerializeField] protected RectTransform favouriteImage;
        [SerializeField] protected Image favImageBorder;

        [SerializeField] protected SubjectList _subList;

        private Teacher teacher;
        protected bool isFav;

        public Teacher Teacher
        {
            get
            {
                return teacher;
            }
        }
        public override void Initialize(params object[] parameters)
        {
            Teacher teacher = parameters[0] as Teacher;
            this.teacher = teacher;

            teacherName.text = teacher.name;
            LoadTeacherData();
            InvokeRepeating("CheckForFile", 0, 0.1f);
        }

        public void LoadTeacherData()
        {
            float rounded = teacher.Rating * 100;
            rounded = Mathf.Round(rounded) / 100;
            rating.text = rounded == 0 ? "N/A" : rounded.ToString();
            _subList.LoadList(teacher.subjects);
        }

        private void CheckForFile()
        {
            if (bytes == null || bytes.Length == 0) return;
            else
            {
                ImageDownloaded(bytes, "");
                CancelInvoke();
                bytes = null;
            }
        }

        public byte[] bytes;

        public void ImageDownloaded(byte[] file, string id)
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
                
                teacherImage.sprite = s;
                teacherImage.preserveAspect = true;
            }
        }

        private void LoadDefaultImg()
        {
            Sprite sp = Resources.Load<Sprite>(Strings.Paths.DefaultProfPic);
            teacherImage.sprite = sp;
        }

        public override void OnClick()
        {
            Core.EventBus.Instance.post<ITeacherProfileEvents>((e, d) => e.LoadTeacher(teacher, teacherImage.sprite));
        }

        public virtual void OnAddToFavourites()
        {
            SetFavourite(!isFav);
            UserManager.Instance.TeacherAddedToFavourites(teacher, isFav);
            EventBus.Instance.post<IFavTeacherListViewEvents>((e, d) => e.FavouritesChanged(Teacher, isFav, teacherImage.sprite));
        }

        public void SetFavourite(bool fav)
        {
            isFav = fav;
            favouriteImage.gameObject.SetActive(isFav);
            favImageBorder.enabled = !isFav;
        }
    }
}
