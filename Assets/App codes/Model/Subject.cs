using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;

namespace DataLayer.Model
{
    public class Subject : BaseModel
    {
        private string name;
        private Faculty faculty;
        private string facultyId;
        
        public Subject() { }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public Faculty Faculty
        {
            get
            {
                return faculty;
            }

            set
            {
                faculty = value;
            }
        }

        public override DbBase GetDbModel()
        {
            return new DbSubject()
            {
                id = this.id,
                name = this.name,
                facultyId = this.faculty.id
            };
        }

        public override void LoadDependentFields()
        {
            this.faculty = _repo.GetModel<Faculty>(facultyId) as Faculty;
        }

        public override void LoadModel(DbBase entity)
        {
            DbSubject subj = entity as DbSubject;
            this.name = subj.name;
            this.id = subj.id;
            this.facultyId = subj.facultyId;
        }
    }
}
