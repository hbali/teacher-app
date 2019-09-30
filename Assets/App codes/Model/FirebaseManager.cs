using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Unity.Editor;
using DataLayer.Database;
using Firebase.Database;
using UnityEngine;

namespace DataLayer.Model
{
    public class FirebaseManager
    {
        private Action<List<DbBase>> entitiesChangedAction;

        public User CurrentUser
        {
            get; set;
        }

        public FirebaseManager()
        {
            //initialization deleted for privacy purposes
        }

        private void ValueChangedEvent(object sender, ValueChangedEventArgs e)
        {
            var children = e.Snapshot.Children;
            Type t = TableNameToType(e.Snapshot.Key);
            List<DbBase> entities = new List<DbBase>();

            if (t == typeof(DbComment))
            {
                foreach (DataSnapshot ds in children)
                {
                    entities.Add(JsonUtility.FromJson<DbComment>(ds.GetRawJsonValue()));
                }
            }
            else if (t == typeof(DbStudent))
            {
                foreach (DataSnapshot ds in children)
                {
                    entities.Add(JsonUtility.FromJson<DbStudent>(ds.GetRawJsonValue()));
                }
            }
            else if (t == typeof(DbTeacher))
            {
                foreach (DataSnapshot ds in children)
                {
                    entities.Add(JsonUtility.FromJson<DbTeacher>(ds.GetRawJsonValue()));
                }
            }
            else if (t == typeof(DbSubject))
            {
                foreach (DataSnapshot ds in children)
                {
                    entities.Add(JsonUtility.FromJson<DbSubject>(ds.GetRawJsonValue()));
                }
            }
            entitiesChangedAction.Invoke(entities);
        }

        private static FirebaseManager instance;
        public static FirebaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FirebaseManager();
                }
                return instance;
            }
        }

        public void GetEntityWithId<T>(string id, Action<T> callBack) where T : DbBase
        {            
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance
             .GetReference(TypeToTableName(typeof(T))).Child(id).GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsCompleted && task.Result.ChildrenCount != 0)
                 {
                     T entity = JsonUtility.FromJson<T>(task.Result.GetRawJsonValue());
                     callBack(entity);
                 }
                 else
                 {
                     callBack(null);
                 }
             }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public void EntityExistsWithFbId<T>(string fbId, Action<bool> callBack) where T : DbBase
        {
            try
            {
                string tableName = typeof(T) == typeof(DbStudent) ? "StudentFacebookId" : "TeacherFacebookId";
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseDatabase.DefaultInstance
                 .GetReference(tableName).Child(fbId).GetValueAsync().ContinueWith(task =>
                 {
                     if (task.IsCompleted && task.Result.Value != null &&
                     !string.IsNullOrEmpty(task.Result.Value.ToString()))
                     {
                         callBack(true);
                     }
                     else
                     {
                         callBack(false);
                     }
                 }, TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                callBack(false);
            }
        }

        public void GetEntityWithFbId<T>(string fbId, Action<T> callBack) where T : DbBase
        {
            string tableName = typeof(T) == typeof(DbStudent) ? "StudentFacebookId" : "TeacherFacebookId";
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;            
            FirebaseDatabase.DefaultInstance
             .GetReference(tableName).Child(fbId).GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsCompleted && task.Result.Value != null &&
                 !string.IsNullOrEmpty(task.Result.Value.ToString()))
                 {
                     GetEntityWithId<T>(task.Result.Value.ToString(), callBack);
                 }
                 else
                 {
                     callBack(null);
                 }
             }, TaskContinuationOptions.ExecuteSynchronously);

        }

        /// <summary>
        /// Pushes a single DbBase entity to the cloud
        /// </summary>
        /// <param name="entity"></param>
        public void PushToCloud<T>(DbBase entity, Action<bool> successCallback = null) where T : DbBase
        {
            string json = JsonUtility.ToJson(entity);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            try
            {
                reference.Child(TypeToTableName(typeof(T))).Child(entity.id).
                    SetRawJsonValueAsync(json).
                    ContinueWith(task =>
                {
                    if(task.IsCompleted && !task.IsFaulted)
                    {
                        if (successCallback != null) successCallback.Invoke(true);
                    }
                    else
                    {
                        if (successCallback != null) successCallback.Invoke(false);
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                if (successCallback != null) successCallback.Invoke(false);
                Debug.LogError(e.ToString());
            }
        }

        public void PushToFbConnectionTable(DbUser entity)
        {
            string json = JsonUtility.ToJson(entity);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            string tableName = entity is DbTeacher ? "TeacherFacebookId" : "StudentFacebookId";
            try
            {
                reference.Child(tableName).Child(entity.facebookId).SetValueAsync(entity.id);
            }
            catch (Exception e)
            {

            }
        }

        public void GetAllTables(Action<List<DbBase>> successAction)
        {
            FirebaseDatabase.DefaultInstance.RootReference
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsCompleted)
                 {
                     DbBase obj;
                     DataSnapshot snapshot = task.Result;
                     var children = snapshot.Children;
                     List<DbBase> entities = new List<DbBase>();
                     foreach (DataSnapshot ds in children)
                     {
                         Type t = TableNameToType(ds.Key);
                         if (t != null)
                         {
                             foreach (DataSnapshot dss in ds.Children)
                             {
                                 obj = JsonUtility.FromJson(dss.GetRawJsonValue(), t) as DbBase;
                                 if (obj != null) entities.Add(obj);
                             }
                         }
                     }
                     successAction(entities);
                 }
                 else
                 {

                 }
             }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public void GetTableFromCloud<T>(Action<List<T>> successAction) where T : DbBase
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance
             .GetReference(TypeToTableName(typeof(T)))
             .GetValueAsync().ContinueWith(task =>
             {
                 if (task.IsCompleted)
                 {
                     DataSnapshot snapshot = task.Result;
                     var children = snapshot.Children;
                     List<T> entities = new List<T>();
                     foreach (DataSnapshot ds in children)
                     {
                         entities.Add(JsonUtility.FromJson<T>(ds.GetRawJsonValue()));
                     }
                     successAction(entities);
                 }
                 else
                 {

                 }
             }, TaskContinuationOptions.ExecuteSynchronously);
        }        

        private string TypeToTableName(Type t)
        {
            if (t == typeof(DbTeacher)) return "Teachers";
            else if (t == typeof(DbSubject)) return "Subjects";
            else if (t == typeof(DbComment)) return "Comments";
            else if (t == typeof(DbStudent)) return "Students";
            else if (t == typeof(DbFaculty)) return "Faculties";
            else if (t == typeof(DbSkill)) return "Skills";
            else return "";
        }

        private Type TableNameToType(string table)
        {
            if (table == "Teachers") return typeof(DbTeacher);
            else if (table == "Students") return typeof(DbStudent);
            else if (table == "Subjects") return typeof(DbSubject);
            else if (table == "Faculties") return typeof(DbFaculty);
            else if (table == "Comments") return typeof(DbComment);
            else if (table == "Skills") return typeof(DbSkill);
            else return null;
        }
    }
}
