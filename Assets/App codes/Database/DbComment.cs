using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Database
{
    public class DbComment : DbBase
    {
        public long timeStamp;
        public string description;
        public string studentId;
        public string subjectId;
        public string teacherId;

        //this is ugly and works this way: 
        //1;4;6;5 means that 1 rating for 1st skill 4 rating for 2nd skill etc etc fast and efficent. sorry.
        public string skillRatings;
    }
}
