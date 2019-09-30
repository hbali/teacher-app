using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database
{
    public class DbTeacher : DbUser
    {
        public List<string> favTeachers;
        public string name;
        public List<string> subjects;
        public string email;
        public bool isMale;
    }
}
