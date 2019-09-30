using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    enum Prefs
    {
        FBTOKEN,
        USERID,
        USERISTEACHER,
        AUTOLOGIN,
        FBID,
        FBNAME,
        ANONSTAT,
        LANGUAGE,
        LANGUAGECHANGED,
        MESSAGINGTOKEN
    }

    public interface IUserPreferencesChangeEvents : IEventSystemHandler
    {
        void UserIdChange(string newId);
    }

    class UserPreferences
    {
        public static string FbToken
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.FBTOKEN.ToString());
            }
            set
            {
                PlayerPrefs.SetString(Prefs.FBTOKEN.ToString(), value);
            }
        }


        public static bool LanguageChanged
        {
            get
            {
                return PlayerPrefs.GetInt(Prefs.LANGUAGECHANGED.ToString()) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(Prefs.LANGUAGECHANGED.ToString(), value ? 1 : 0);
            }
        }

        public static string Language
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.LANGUAGE.ToString(), "Hungarian");
            }
            set
            {
                PlayerPrefs.SetString(Prefs.LANGUAGE.ToString(), value);
            }
        }

        public static string CurrentUserId
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.USERID.ToString());
            }
            set
            {
                PlayerPrefs.SetString(Prefs.USERID.ToString(), value);
                EventBus.Instance.post<IUserPreferencesChangeEvents>((e, d) => e.UserIdChange(value));
            }
            
        }
        public static string MessagingToken
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.MESSAGINGTOKEN.ToString());
            }
            set
            {
                PlayerPrefs.SetString(Prefs.MESSAGINGTOKEN.ToString(), value);
            }

        }

        public static bool CurrentUserIsTeacher
        {
            get
            {
                return PlayerPrefs.GetInt(Prefs.USERISTEACHER.ToString()) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(Prefs.USERISTEACHER.ToString(), value ? 1 : 0);
            }
        }

        public static bool AutoLog
        {
            get
            {
                return PlayerPrefs.GetInt(Prefs.AUTOLOGIN.ToString()) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(Prefs.AUTOLOGIN.ToString(), value ? 1 : 0);
            }
        }
        public static bool AnonymousStat
        {
            get
            {
                return PlayerPrefs.GetInt(Prefs.ANONSTAT.ToString()) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(Prefs.ANONSTAT.ToString(), value ? 1 : 0);
            }
        }

        public static string FbId
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.FBID.ToString());
            }
            set
            {
                PlayerPrefs.SetString(Prefs.FBID.ToString(), value);
            }
        }

        public static string FbName
        {
            get
            {
                return PlayerPrefs.GetString(Prefs.FBNAME.ToString());
            }
            set
            {
                PlayerPrefs.SetString(Prefs.FBNAME.ToString(), value);
            }
        }
    }
}
