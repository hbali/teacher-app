using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Database;

namespace DataLayer.Model
{
    public class Faculty : BaseModel
    {
        private string name;
        
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

        public override DbBase GetDbModel()
        {
            return new DbFaculty()
            {
                name = this.Name,
                id = this.id
            };
        }

        public override void LoadDependentFields() { }

        public override void LoadModel(DbBase entity)
        {
            DbFaculty fac = entity as DbFaculty;
            this.Name = fac.name;
            this.id = fac.id;
        }
    }
}
