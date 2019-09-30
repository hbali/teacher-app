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
    public abstract class BaseCompactVerticalListView<T> : BaseCompactListView<T> where T : BaseCompactListItem
    {
        /// <summary>
        /// Destroys the item and decreases the Y sizeDelta of the itemContainer
        /// </summary>
        /// <param name="item">Item to destroy</param>
        protected override void DestroySingleItem(T item)
        {
            base.DestroySingleItem(item);
            itemContainer.sizeDelta -= new Vector2(0, ItemSize);
        }

        protected override void TurnOffSingleElement(T item)
        {
            if (item.gameObject.activeSelf)
            {
                itemContainer.sizeDelta -= new Vector2(0, ItemSize);
                base.TurnOffSingleElement(item);
            }
        }

        protected override void TurnOnSingleElement(T item)
        {
            if (!item.gameObject.activeSelf)
            {
                itemContainer.sizeDelta += new Vector2(0, ItemSize);
                base.TurnOnSingleElement(item);
            }
        }

        /// <summary>
        /// Creates a single item with T component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesPath"></param>
        /// <returns></returns>
        protected override T CreateSingleItem(string resourcesPath)
        {
            itemContainer.sizeDelta += new Vector2(0, ItemSize);
            return base.CreateSingleItem(resourcesPath);
        }
    }
}
