using Core;
using DataLayer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UI.TeacherProfile;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DataLayer.Model
{
    public class Repository : IRepository
    {
        public bool IsInitialized
        {
            get;
            private set;
        }

        private Dictionary<string, Student> students;
        private Dictionary<string, Teacher> teachers;
        private Dictionary<string, Subject> subjects;
        private Dictionary<string, Faculty> faculties;
        private Dictionary<string, Comment> comments;
        private Dictionary<string, Skill> skills;

        public Repository()
        {
            students = new Dictionary<string, Student>();
            teachers = new Dictionary<string, Teacher>();
            subjects = new Dictionary<string, Subject>();
            faculties = new Dictionary<string, Faculty>();
            comments = new Dictionary<string, Comment>();
            skills = new Dictionary<string, Skill>();
        }
        
        public IEnumerable<T> GetAllModelsOfType<T>() where T : BaseModel
        {
            return (GetDictionaryForModel<T>() as Dictionary<string, T>).Values;
        }

        /// <summary>
        /// Adds a model to the propert dictionary. If already exists, refreshes the model to the current
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void AddModel<T>(T model) where T : BaseModel
        {
            (GetDictionaryForModel<T>() as Dictionary<string, T>)[model.id] = model;
        }

        public void AddDbModel<T>(DbBase model) where T : BaseModel
        {
            CreateModel<T>(model).LoadDependentFields();
        }

        /// <summary>
        /// Initializes the dependant fields of the model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public void RefreshModel<T>(string id) where T : BaseModel
        {
            (GetDictionaryForModel<T>() as Dictionary<string, T>)[id].LoadDependentFields();
        }

        public void Initialize()
        {
            FirebaseManager.Instance.GetAllTables(DatabaseFetched);
            /*FirebaseManager.Instance.GetTableFromCloud<DbSkill>(SuccessSkillDownload);
            FirebaseManager.Instance.GetTableFromCloud<DbFaculty>(SuccessFacultiesDownload);
            FirebaseManager.Instance.GetTableFromCloud<DbSubject>(SuccessSubjectsDownload);
            FirebaseManager.Instance.GetTableFromCloud<DbTeacher>(SuccessTeachersDownload);
            FirebaseManager.Instance.GetTableFromCloud<DbComment>(SuccessCommentDownload);
            FirebaseManager.Instance.GetTableFromCloud<DbStudent>(SuccessStudentsDownload);*/
        }

        private void DatabaseFetched(List<DbBase> entities)
        {
            foreach(DbBase entity in entities)
            {
                if (entity is DbSkill) CreateModel<Skill>(entity);
                else if (entity is DbStudent) CreateModel<Student>(entity);
                else if (entity is DbTeacher) CreateModel<Teacher>(entity);
                else if (entity is DbFaculty) CreateModel<Faculty>(entity);
                else if (entity is DbSubject) CreateModel<Subject>(entity);
                else if (entity is DbComment) CreateModel<Comment>(entity);
            }
            InitializeSuccess();
        }
        
        private void/*IEnumerable<BaseModel>*/ CreateModels<T>(IEnumerable<DbBase> entities) where T : BaseModel
        {
            //List<BaseModel> models = new List<BaseModel>();
            foreach(DbBase entity in entities)
            {
                try
                {
                    T model = CreateModel<T>(entity);
                    //models.Add(model);
                }
                catch (Exception e)
                {
                    Debug.Log("");
                }
            }
            //return models;
        }

        private T CreateModel<T>(DbBase entity) where T : BaseModel
        {
            T model = Activator.CreateInstance<T>();
            model.LoadModel(entity);
            (GetDictionaryForModel<T>() as Dictionary<string, T>).Add(entity.id, model);
            return model;
        }

        private void InitializeSuccess()
        {
            //System.Threading.Thread t = new Thread(() =>
            {
                InitializeDependentFields(skills.Values.Select(x => x as BaseModel));
                InitializeDependentFields(comments.Values.Select(x => x as BaseModel));
                InitializeDependentFields(faculties.Values.Select(x => x as BaseModel));
                InitializeDependentFields(subjects.Values.Select(x => x as BaseModel));
                InitializeDependentFields(students.Values.Select(x => x as BaseModel));
                InitializeDependentFields(teachers.Values.Select(x => x as BaseModel));
            }//);
             // t.Start();
            IsInitialized = true;
            /*FirebaseManager.Instance.InitializeValueChangedCallbacks<DbComment>(ValueChanged);
            FirebaseManager.Instance.InitializeValueChangedCallbacks<DbTeacher>(ValueChanged);
            FirebaseManager.Instance.InitializeValueChangedCallbacks<DbStudent>(ValueChanged);
            FirebaseManager.Instance.InitializeValueChangedCallbacks<DbSubject>(ValueChanged);*/

            EventBus.Instance.post<IRepositoryEvents>((e, d) => e.RepoInitialized());
        }
        
        private void InitializeDependentFields(IEnumerable<BaseModel> models)
        {
            foreach(BaseModel bm in models)
            {
                bm.LoadDependentFields();
            }
        }

        public T GetModel<T>(string id) where T : BaseModel
        {
            Type t = typeof(T);
            Dictionary<string, T> dict = GetDictionaryForModel<T>() as Dictionary<string, T>;
            T value = null;
            dict.TryGetValue(id, out value);
            return value;
        }

        private object GetDictionaryForModel<T>() where T : BaseModel
        {
            Type t = typeof(T);
            if (t == typeof(Teacher)) return teachers;
            else if (t == typeof(Student)) return students;
            else if (t == typeof(Faculty)) return faculties;
            else if (t == typeof(Subject)) return subjects;
            else if (t == typeof(Comment)) return comments;
            else if (t == typeof(Skill)) return skills;
            else
            {
                throw new ArgumentException("Type {0} is invalid", t.ToString());
            }
        }

        #region function methods

        public bool ContainsSubjectWithFaculty(string subjectId, string facultyId)
        {
            return subjects[subjectId].Faculty.id == facultyId;
        }


        #endregion
    }

    public interface IRepository
    {
        void AddDbModel<T>(DbBase model) where T : BaseModel;

        void AddModel<T>(T model) where T : BaseModel;
        T GetModel<T>(string id) where T : BaseModel;
        bool IsInitialized
        {
            get;
        }

        IEnumerable<T> GetAllModelsOfType<T>() where T : BaseModel;
        void Initialize();
        bool ContainsSubjectWithFaculty(string subjectId, string facultyId);
        void RefreshModel<T>(string id) where T : BaseModel;
    }

    public interface IRepositoryEvents : IEventSystemHandler
    {
        void RepoInitialized();
    }
}
