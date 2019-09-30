using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class EventBus : ScriptableObject
    {
        #region singleton
        private static EventBus instance;

        public static EventBus Instance
        {
            get { Initialize(); return instance; }
        }

        public static void Initialize()
        {
            if (instance == null)
                instance = ScriptableObject.CreateInstance<EventBus>();
        }
        #endregion

        #region private fields
        List<KeyValuePair<GameObject, Type>> list = new List<KeyValuePair<GameObject, Type>>();
        #endregion

        #region public methods
        public void register<T>(GameObject obj)
        {
            list.Add(new KeyValuePair<GameObject, Type>(obj, typeof(T)));
        }

        public void unregister(GameObject obj)
        {
            list.RemoveAll(a => a.Key == obj);
        }

        public void post<T>(ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        {
            var lst = list.Where(a => a.Value == typeof(T)).Select(a => a.Key).ToList();
            foreach (var obj in lst)
            {
                if (obj != null)
                {
                    ExecuteEvents.Execute<T>(obj, null, func);
                }
            }
        }

        public static void postChildren<T>(GameObject root, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        {
            var lst = root.GetComponentsInChildren<MonoBehaviour>().Where(a => a is T).ToArray();
            foreach (var child in lst)
            {
                ExecuteEvents.Execute<T>(child.gameObject, null, func);
            }
        }

        public static void postChildren<T>(MonoBehaviour root, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        {
            postChildren<T>(root.gameObject, func);
        }

        public static void postParent<T>(GameObject root, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        {
            ExecuteEvents.ExecuteHierarchy<T>(root, null, func);
        }

        public static void postParent<T>(MonoBehaviour root, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
        {
            ExecuteEvents.ExecuteHierarchy<T>(root.gameObject, null, func);
        }
        #endregion
    }
}

