using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI.CompactListView
{
    /// <summary>
    /// Base abstract class for creating vertical listviews
    /// </summary>
    public abstract class BaseCompactListView<T> : MonoBehaviour where T : BaseCompactListItem
    {
        [SerializeField] protected RectTransform itemContainer;
        protected List<T> itemList;

        protected virtual void Awake()
        {
            if(itemList == null)
                itemList = new List<T>();
        }

        protected virtual void TurnOffSingleElement(T item)
        {
            item.gameObject.SetActive(false);
        }

        protected virtual void TurnOnSingleElement(T item)
        {
            item.gameObject.SetActive(true);
        }

        protected virtual float ItemSize
        {
            get
            {
                return 50;
            }
        }

        /// <summary>
        /// Destroys all items
        /// </summary>
        protected virtual void DestroyAllItems()
        {
            if (itemList != null)
            {
                for (int i = itemList.Count - 1; i >= 0; i--)
                {
                    DestroySingleItem(itemList[i]);
                }
            }
        }

        /// <summary>
        /// Destroys the item and decreases the Y sizeDelta of the itemContainer
        /// </summary>
        /// <param name="item">Item to destroy</param>
        protected virtual void DestroySingleItem(T item)
        {
            itemList.Remove(item);
            DestroyImmediate(item.gameObject);
        }

        /// <summary>
        /// Creates a single item with T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesPath"></param>
        /// <returns></returns>
        protected virtual T CreateSingleItem(string resourcesPath)
        {
            if (itemList == null)
                itemList = new List<T>();
            GameObject go = Instantiate(Resources.Load<GameObject>(resourcesPath));
            go.transform.SetParent(itemContainer, false);
            T item = go.GetComponent<T>();
            itemList.Add(item);
            return item;
        }
    }
}
