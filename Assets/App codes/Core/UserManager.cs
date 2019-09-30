using DataLayer.Database;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance
        {
            get
            {
                if (instance == null) instance = new UserManager();
                return instance;
            }
        }

        IRepository _repo;

        public User CurrentUser
        {
            get
            {
                if (_repo.IsInitialized)
                {
                    return UserPreferences.CurrentUserIsTeacher ? _repo.GetModel<Teacher>(UserPreferences.CurrentUserId) :
                        _repo.GetModel<Student>(UserPreferences.CurrentUserId) as User;
                }
                else return null;
            }
        }
        public void SetRepo(IRepository repo)
        {
            this._repo = repo;
        }

        public void TeacherAddedToFavourites(Teacher teacher, bool isFav)
        {
            if (isFav) CurrentUser.favTeachers.Add(teacher.id);
            else CurrentUser.favTeachers.Remove(teacher.id);
            if (CurrentUser is Teacher)
                FirebaseManager.Instance.PushToCloud<DbTeacher>(CurrentUser.GetDbModel());
            else
                FirebaseManager.Instance.PushToCloud<DbStudent>(CurrentUser.GetDbModel());
        }
    }
}
