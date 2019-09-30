using DataLayer.Model;
using UnityEngine;

namespace UI.CompactListView
{
    public abstract class BaseCompactListItem : MonoBehaviour
    {
        public abstract void Initialize(params object[] parameters);
        public abstract void OnClick();
    }
}