using Core;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI.Base;
using UI.CompactListView;
using UnityEngine;

namespace UI.CommentsView
{
    public class CommentView : BaseCompactVerticalListView<CommentItem>
    {
        private List<Comment> comments;

        private bool enableReport;

        [SerializeField] private RectTransform noComments;

        protected override float ItemSize
        {
            get
            {
                return 120;
            }
        }

        public void LoadComments(List<Comment> comments, bool enableReport)
        {
            DestroyAllItems();
            if (comments.Count == 0)
            {
                ShowNoCommentPanel();
            }
            else
            {
                noComments.gameObject.SetActive(false);
                this.comments = comments.OrderByDescending(x => x.timeStamp).ToList();
                LoadSomeComments(this.comments.Count, enableReport);
                DownloadPictures();
            }
        }

        private void DownloadPictures()
        {
            Thread thread = new Thread(() =>
            {
                foreach (Comment c in comments)
                {
                    StorageManager.Instance.GetPictureFor(c.StudentId, FileRead, true);
                }
            });
            thread.Start();
        }

        private void FileRead(byte[] file, string id)
        {
            itemList.Where(x => x.comment.StudentId == id).ToList().ForEach(x => x.bytes = file);
        }

        private void ShowNoCommentPanel()
        {
            noComments.gameObject.SetActive(true);
        }

        public void OnClose()
        {
            GetComponentInParent<BasePopup>().OnClose();
        }

        private void LoadSomeComments(int count, bool enableReport)
        {
            for (int i = 0; i < count; i++)
            {
                var item = CreateSingleItem("Prefabs/Comments/CommentItem");
                item.Initialize(comments[i]);
                item.EnableReport(enableReport);
            }
        }
    }
}
