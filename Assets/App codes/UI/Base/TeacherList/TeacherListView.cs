using Core;
using DataLayer.Model;
using MovementEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UI.CompactListView;
using UnityEngine;

namespace UI.Base.TeacherList
{
    public abstract class TeacherListView<T> : BaseCompactVerticalListView<T> where T : TeacherListItem
    {
        private List<Teacher> teachers;
        public bool listLoaded;

        protected abstract string PrefabPath
        {
            get;
        }

        protected override float ItemSize
        {
            get
            {
                return 105;
            }
        }
        public void TeachersDownloaded(List<Teacher> teachers, User currentUser)
        {
            this.teachers = teachers;
            for (int i = 0; i < teachers.Count; i++)
            {
                if (itemList != null && itemList.Where(x => x.Teacher.id == teachers[i].id).Count() == 0)
                {
                    TeacherListItem item = CreateSingleItem(PrefabPath);
                    item.Initialize(teachers[i], this);
                    if (currentUser != null && currentUser.favTeachers.Contains(teachers[i].id))
                    {
                        item.SetFavourite(true);
                    }
                    else
                    {
                        item.SetFavourite(false);
                    }
                }
            }
            listLoaded = true;

            Thread thread = new Thread(() =>
            {
                foreach(Teacher t in teachers)
                {
                    StorageManager.Instance.GetPictureFor(t.id, FileRead, true);
                }
            });
            thread.Start();
        }

        private void FileRead(byte[] file, string id)
        {
            itemList.Where(x => x.Teacher.id == id).FirstOrDefault().bytes = file;
        }

        internal void Filter(IEnumerable<Teacher> matchedTeachers)
        {
            TurnOffElements(matchedTeachers);
        }

        private void TurnOffElements(IEnumerable<Teacher> matchedTeachers)
        {
            Teacher[] ordered = matchedTeachers.ToArray();
            for (int i = 0; i < ordered.Length; i++)
            {
                T item = itemList.Where(x => x.Teacher == ordered[i]).FirstOrDefault();
                item.gameObject.GetComponent<RectTransform>().SetSiblingIndex(i);
            }
            foreach (T t in itemList)
            {
                Teacher item = matchedTeachers.Where(x => x == t.Teacher).FirstOrDefault();
                if (item != null) TurnOnSingleElement(t);
                else TurnOffSingleElement(t);
            }
        }

        public void UnloadElement(T item)
        {
            if (item != null)
                DestroySingleItem(item);
        }
    }
}
