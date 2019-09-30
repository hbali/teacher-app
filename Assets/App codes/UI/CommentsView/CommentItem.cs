using Core;
using DataLayer.Model;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI.CompactListView;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CommentsView
{
    public class CommentItem : BaseCompactListItem
    {
        [SerializeField] private Text commentText;
        [SerializeField] private Image commenterImage;
        [SerializeField] private RectTransform reportButton;

        public Comment comment;

        public override void Initialize(params object[] parameters)
        {
            this.comment = parameters[0] as Comment;

            string txt = "";
            foreach (KeyValuePair<Skill, int> kvp in comment.SkillRatings)
            {
                txt += kvp.Key.Name + ": " + kvp.Value + " ";
            }
            commentText.text = txt + "\n\n" + comment.Description;
            InvokeRepeating("CheckForFile", 0, 0.1f);
        }

        private void CheckForFile()
        {
            if (bytes == null || bytes.Length == 0) return;
            else
            {
                ImageDownloaded(bytes);
                CancelInvoke();
                bytes = null;
            }
        }

        public byte[] bytes;

        private void ImageDownloaded(byte[] file)
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

                commenterImage.sprite = s;
                commenterImage.preserveAspect = true;
            }
            file = null;
        }
        private void LoadDefaultImg()
        {
            Sprite sp = Resources.Load<Sprite>(Strings.Paths.DefaultProfPic);
            commenterImage.sprite = sp;
        }

        public void EnableReport(bool enable)
        {
            reportButton.gameObject.SetActive(enable);
        }

        public void OnReport()
        {
            MailExtensions.ReportComment(comment);
        }

        public override void OnClick()
        {

        }
    }
}
