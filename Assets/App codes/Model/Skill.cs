using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;
using Core;

namespace DataLayer.Model
{
    public class Skill : BaseModel
    {
        private const string postFixDescriptionLocalization = "_description";
        private string name;
        private string description;

        public string Name
        {
            get
            {
                return Strings.GetString(name);
            }
        }
        public string Description
        {
            get
            {
                return Strings.GetString(name + postFixDescriptionLocalization);
            }
        }
        public override DbBase GetDbModel()
        {
            return new DbSkill()
            {
                name = this.name,
                id = this.id
            };
        }

        public override void LoadDependentFields()
        {

        }

        public override void LoadModel(DbBase entity)
        {
            DbSkill skill = entity as DbSkill;
            this.id = skill.id;
            this.name = skill.name;
        }
    }
}
