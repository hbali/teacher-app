using DataLayer.Model;
using UnityEngine;

namespace UI.Base
{
    public abstract class BaseView : MonoBehaviour
    {
        protected IRepository _repo;

        internal void SetDependencies(IRepository repo)
        {
            this._repo = repo;
        }

        public abstract void OnMenuSelected();
        public abstract void OnMenuDeselected();
    }
}