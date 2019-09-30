using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Facebook.Unity;
using Core;
using UnityEngine.EventSystems;

namespace UI.LoginView
{
    class Login
    {
        #region SDK codes

        public string FbToken { get { return UserPreferences.FbToken; } }
        public string FbId { get { return UserPreferences.FbId; } }
        public string FbName { get { return UserPreferences.FbName; } }
        
        public void InitializeAuth()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            InitiateFacebookLogin("public_profile", "email");

            if (FB.IsInitialized)
            {                
                FB.ActivateApp();
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        private void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                AccessToken aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                UserPreferences.FbToken = aToken.TokenString;
                UserPreferences.FbId = result.ResultDictionary["user_id"].ToString();
                EventBus.Instance.post<ILoginEvents>((e, d) => e.SuccessFacebookLogin());
            }
            else
            {
                Utilities.MessagePopupManager.Instance.StopLoader();
                EventBus.Instance.post<ILoginEvents>((e, d) => e.FailedFacebookLogin());
            }
        }
        #endregion
        
        /// <summary>
        /// Initiates a facebook login with the permissions in the parameter
        /// </summary>
        /// <param name="permissions">permissions to initiate the FB login</param>
        public void InitiateFacebookLogin(params string[] permissions)
        {
            FB.LogInWithReadPermissions(permissions, AuthCallback);
        }        
    }

    public interface ILoginEvents : IEventSystemHandler
    {
        void SuccessFacebookLogin();
        void FailedFacebookLogin();
    }

}
