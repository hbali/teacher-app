using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database
{
    public class DbStudent : DbUser
    {
        public List<string> favTeachers;
        public string name;
        public string email;
    }
}
