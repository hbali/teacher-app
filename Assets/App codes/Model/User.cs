

using System.Collections.Generic;

namespace DataLayer.Model
{
    public abstract class User : BaseModel
    {
        public List<string> favTeachers;
        public string name;
        public string messagingToken;
    }
}
