using DataLayer.Database;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DataLayer.Model
{
    public class Student : User
    {
        public string facebookId;
        public string email;
        public List<Comment> comments;
        
        public Student()
        {
            favTeachers = new List<string>();
        }

        public List<string> FavTeachers
        {
            get
            {
                return favTeachers;
            }

            set
            {
                favTeachers = value;
            }
        }

        public override DbBase GetDbModel()
        {
            DbStudent stud = new DbStudent()
            {
                id = this.id,
                favTeachers = this.favTeachers,
                facebookId = this.facebookId,
                name = this.name,
                email = this.email
            };
            return stud;
        }

        public override void LoadDependentFields()
        {
            comments = _repo.GetAllModelsOfType<Comment>().Where(x => x.StudentId == this.id).ToList();
        }

        public override void LoadModel(DbBase entity)
        {
            this.messagingToken = (entity as DbUser).messagingToken;
            this.facebookId = (entity as DbStudent).facebookId;
            this.id = entity.id;
            this.email = (entity as DbStudent).email;
            this.favTeachers = (entity as DbStudent).favTeachers;
            this.name = (entity as DbStudent).name;
        }
    }
}