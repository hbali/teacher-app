using System;
using DataLayer.Database;

namespace DataLayer.Model
{
    public abstract class BaseModel
    {
        public bool deleted;
        public string id;
        protected static IRepository _repo;        
        
        public abstract DbBase GetDbModel();
        public abstract void LoadModel(DbBase entity);
        public abstract void LoadDependentFields();

        internal static void SetDependencies(IRepository repo)
        {
            _repo = repo;
        }
    }
}