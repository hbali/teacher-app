using Firebase;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class StorageManager
    {
        private string NORMALSIZECACHE = "Pictures";
        public readonly string PersistentdDataPath;
        public readonly string DataPath;

        private static StorageManager instance;
        public static StorageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageManager();
                }
                return instance;
            }
        }

        private Firebase.Storage.FirebaseStorage storage;
        Firebase.Storage.StorageReference storage_ref;
        public StorageManager()
        {
            DataPath = Application.dataPath;
            PersistentdDataPath = Application.persistentDataPath;
            CheckDirectories();
            //initialization deleted for privacy purposes
        }

        public void GetPictureFor(string id, Action<byte[], string> successCallback, bool isThumb)
        {            
            string path = "";
            if (!TryGetPic(id, isThumb, out path))
            {
                Firebase.Storage.StorageReference picRef = storage_ref.Child
                 ("Pictures/" + (isThumb ? "thumb_" + id + ".png" : id + ".png"));

                string local_url = GetFilePath(id, isThumb);

                picRef.GetFileAsync(local_url).ContinueWith(task => 
                {
                    if (!task.IsFaulted && !task.IsCanceled)
                    {
                        byte[] file = ReadImageFromLocal(path);
                        successCallback(file, id);
                    }
                    else
                    {
                        successCallback(null, id);
                    }
                });
            }
            else
            {
                byte[] file = ReadImageFromLocal(path);
                successCallback(file, id);
            }
        }

        public void UploadImage(byte[] file, string id, bool isThumb)
        {
            Firebase.Storage.StorageReference picRef = storage_ref.Child("Pictures/" + id + ".png");

            try
            {
                picRef.PutBytesAsync(file)
                  .ContinueWith((task) =>
                  {
                      if (task.IsFaulted || task.IsCanceled)
                      {
                          Debug.Log(task.Exception.ToString());
                      }
                      else
                      {
                          //success
                      }
                  });
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
            }
            SaveImage(file, id, isThumb);
        }

        private void SaveImage(byte[] file, string id, bool isThumb)
        {
            string path= GetFilePath(id, isThumb);

            if(!File.Exists(path))
            {
                File.WriteAllBytes(path, file);
            }
        }

        private void CheckDirectories()
        {
            CreateDirectory(NORMALSIZECACHE);
        }

        private byte[] ReadImageFromLocal(string path)
        {
            if (File.Exists(path))
            {
                byte[] file = File.ReadAllBytes(path);
                return file;
            }
            else
            {
                throw new ArgumentException("File doesn't exist on path: " + path);
            }
        }

        private void CreateDirectory(string dir)
        {
            string path = string.IsNullOrEmpty(PersistentdDataPath) ?
                           Path.Combine(DataPath, dir) :
                           Path.Combine(PersistentdDataPath, dir);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private bool TryGetPic(string id, bool isThumb, out string path)
        {
            path = GetFilePath(id, isThumb);
            return File.Exists(path);
        }

        private string GetFilePath(string id, bool isThumb)
        {
            return string.IsNullOrEmpty(PersistentdDataPath) ?
                                       Path.Combine(Path.Combine(DataPath, NORMALSIZECACHE), isThumb ? "thumb_" + id + ".png": id + ".png") :
                                       Path.Combine(Path.Combine(PersistentdDataPath, NORMALSIZECACHE), isThumb ? "thumb_" + id + ".png" : id + ".png");
        }

    }
}
