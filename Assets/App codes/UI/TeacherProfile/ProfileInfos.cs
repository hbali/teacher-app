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

namespace UI.TeacherProfile
{
    public class ProfileInfos : MonoBehaviour
    {
        [SerializeField] private Text teacherName;
        [SerializeField] private Text rating;
        [SerializeField] private Image teacherImage;
        [SerializeField] private Image favouriteImage;
        [SerializeField] private RectTransform ratingHolder;

        private Teacher teacher;
        private bool isFav;

        public Teacher Teacher
        {
            get
            {
                return teacher;
            }
        }

        public void Initialize(Teacher model, Sprite img, bool isFav)
        {
            this.teacher = model;
            SetRating(teacher.Rating);
            SetFavourite(isFav);
            Initialize(model, img);
        }

        private void SetRating(float notRounded)
        {
            ratingHolder.gameObject.SetActive(true);
            float rounded = notRounded * 100;
            rounded = Mathf.Round(rounded) / 100;
            rating.text = rounded == 0 ? "N/A" : rounded.ToString();
        }

        public void Initialize(User model, Sprite img)
        {
            if (model is Teacher) SetRating((model as Teacher).Rating);
            else ratingHolder.gameObject.SetActive(false);
            teacherImage.sprite = img ?? teacherImage.sprite;
            teacherImage.preserveAspect = true;
            teacherName.text = model.name;
        }

        public void SetPicture(Sprite img)
        {
            teacherImage.sprite = img ?? teacherImage.sprite;
        }

        public void SetFavourite(bool fav)
        {
            isFav = fav;
            favouriteImage.gameObject.SetActive(fav);
        }
    }
}
