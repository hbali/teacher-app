using Facebook.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    class FacebookUtilities
    {
        private static FacebookUtilities instance;
        private Action<string> failureCallback;
        private Action<Texture2D> successPictureCallback;
        private Action<IDictionary<string, object>> successProfileDataCallback;

        public static FacebookUtilities Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FacebookUtilities();
                }
                return instance;
            }
        }

        
        public void GetProfilePicture(Action<Texture2D> successCallback, Action<string> failureCallback)
        {
            this.successPictureCallback = successCallback;
            this.failureCallback = failureCallback;
            FB.API("/me/picture?width=500&height=500", HttpMethod.GET, this.ProfilePhotoCallback);
        }

        private void ProfilePhotoCallback(IGraphResult result)
        {
            if (result.Texture != null)
            {
                successPictureCallback.Invoke(result.Texture);
            }
            else
            {
                failureCallback.Invoke(result.Error);
            }
        }

        public void GetProfileData(Action<IDictionary<string, object>> successCallback, Action<string> failureCallback)
        {
            this.failureCallback = failureCallback;
            this.successProfileDataCallback = successCallback;
            FB.API("/me?fields=id,gender,email,name", HttpMethod.GET, this.HandleResult);
        }

        private void HandleResult(IGraphResult result)
        {
            if (result.ResultDictionary != null)
            {
                successProfileDataCallback.Invoke(result.ResultDictionary);
            }
            else
            {
                failureCallback.Invoke(result.Error);
            }
        }
    }
}
