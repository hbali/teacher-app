using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;

namespace DataLayer.Model
{
    public class Comment : BaseModel
    {
        public long timeStamp;
        private string description;
        private string studentId;
        private string teacherId;
        private string subjectId;
        private string skillRatingIds;

        private float rating;
        private Student student;
        private Teacher teacher;
        private Subject subject;
        private Dictionary<Skill, int> skillRatings;
        
        public Comment()
        {
        }
        public string TeacherId
        {
            get
            {
                return teacherId;
            }
        }

        /// <summary>
        /// Returns the avarage rating of the skills
        /// </summary>
        public float Rating
        {
            get
            {
                return (float)SkillRatings.Average(x => x.Value);
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }
        public string SubjectId
        {
            get
            {
                return subjectId;
            }
        }

        public Dictionary<Skill, int> SkillRatings
        {
            get
            {
                return skillRatings;
            }

            set
            {
                skillRatings = value;
            }
        }

        public string StudentId
        {
            get
            {
                return studentId;
            }
        }

        public Subject Subject
        {
            get
            {
                return _repo.GetModel<Subject>(subjectId);
            }
        }

        public override DbBase GetDbModel()
        {
            return null;
        }

        public override void LoadDependentFields()
        {
            student = _repo.GetModel<Student>(studentId) as Student;
            teacher = _repo.GetModel<Teacher>(teacherId) as Teacher;
            subject = _repo.GetModel<Subject>(subjectId) as Subject;
            string[] ratings = skillRatingIds.Split(';');
            for (int i = 0; i < ratings.Length; i++)
            {
                int idx = i + 1;
                skillRatings.Add(_repo.GetModel<Skill>("s" + idx), int.Parse(ratings[i]));
            }
        }

        public override void LoadModel(DbBase entity)
        {
            skillRatings = new Dictionary<Skill, int>();
            DbComment comment = entity as DbComment;
            this.id = comment.id;
            this.studentId = comment.studentId;
            this.teacherId = comment.teacherId;
            this.subjectId = comment.subjectId;
            this.description = comment.description;
            this.skillRatingIds = comment.skillRatings;
            this.timeStamp = comment.timeStamp;
        }
    }
}
