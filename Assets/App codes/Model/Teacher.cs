using DataLayer.Database;
using System.Collections.Generic;
using System;
using System.Linq;
using Core;
using UI.TeacherProfile;

namespace DataLayer.Model
{
    public class Teacher : User
    {
        public List<Subject> subjects;
        public List<Comment> comments;
        public Dictionary<Skill, float> skillRatings;
        public string fbId;
        private float rating;
        public string email;
        public bool isMale;
        private List<string> subjectIds;
        public string messengerId;
        
        public Teacher()
        {
            Init();
        }

        private void Init()
        {
            subjects = new List<Subject>();
            comments = new List<Comment>();
            skillRatings = new Dictionary<Skill, float>();
        }

        public float Rating
        {
            get
            {
                return rating;
            }
        }

        public List<Faculty> Faculties
        {
            get
            {
                return subjects == null ? new List<Faculty>() : subjects.Select(x => x.Faculty).Distinct().ToList();
            }
        }

        public void AddSubjects(List<Subject> subjects)
        {
            this.subjects.AddRange(subjects);
            this.subjectIds.AddRange(subjects.Select(x => x.id));
            EventBus.Instance.post<ITeacherChangedEvents>((e, d) => e.TeacherChanged(id));
        }

        private List<string> GetDbSubjects()
        {
            return subjects.Select(x => x.id).ToList();
        }
        private List<Subject> GetModelSubjects(List<string> dbSubs)
        {
            return null;
        }

        public override DbBase GetDbModel()
        {
            DbTeacher teacher = new DbTeacher()
            {
                subjects = GetDbSubjects(),
                name = name,
                id = id,
                facebookId = fbId,
                email = this.email,
                isMale = this.isMale,
                favTeachers = this.favTeachers,
                messengerId = this.messengerId
            };
            return teacher;
        }

        public override void LoadModel(DbBase entity)
        {
            DbTeacher teacher = entity as DbTeacher;
            this.name = teacher.name;
            this.id = teacher.id;
            this.subjectIds = teacher.subjects;
            this.fbId = teacher.facebookId;
            this.email = teacher.email;
            this.isMale = teacher.isMale;
            this.favTeachers = teacher.favTeachers;
            this.messagingToken = (entity as DbUser).messagingToken;
            this.messengerId = teacher.messengerId;
        }

        internal void RemoveSubject(Subject subject)
        {
            subjects.Remove(subject);
            subjectIds.Remove(subject.id);
            EventBus.Instance.post<ITeacherChangedEvents>((e, d) => e.TeacherChanged(id));
        }

        public override void LoadDependentFields()
        {
            Init();
            foreach(string id in subjectIds)
            {
                this.subjects.Add(_repo.GetModel<Subject>(id) as Subject);
            }
            comments = _repo.GetAllModelsOfType<Comment>().Where(x => x.TeacherId == this.id && subjects.Contains(x.Subject)).ToList();
            if (comments.Count != 0)
            {
                rating = comments.Sum(x => x.Rating) / comments.Count;
            }
            else
            {
                rating = 0;
            }
            CreateRatings();
        }

        private void CreateRatings()
        {
            if (comments.Count > 0)
            {
                CreateRatingFromComments();
            }
            else
            {
                CreateDefaultRatings();
            }
        }

        private void CreateDefaultRatings()
        {
            foreach (Skill s in _repo.GetAllModelsOfType<Skill>())
            {
                skillRatings.Add(s, 0);
            }
        }

        private void CreateRatingFromComments()
        {
            foreach (Comment c in comments)
            {
                foreach (KeyValuePair<Skill, int> kvp in c.SkillRatings)
                {
                    TryAddSkillRating(kvp);
                }
            }
            foreach (Skill s in _repo.GetAllModelsOfType<Skill>())
            {
                skillRatings[s] /= comments.Count;
            }
        }

        private void TryAddSkillRating(KeyValuePair<Skill, int> kvp)
        {
            if(!skillRatings.ContainsKey(kvp.Key))
            {
                skillRatings.Add(kvp.Key, kvp.Value);
            }
            else
            {
                skillRatings[kvp.Key] += kvp.Value;
            }
        }
    }
}