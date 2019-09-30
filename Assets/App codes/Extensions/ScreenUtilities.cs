using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UI;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace UI.Utilities
{
    public class ScreenUtilities
    {
        #region singleton
        static ScreenUtilities instance;

        public static ScreenUtilities Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenUtilities();
                return instance;
            }
        }
        #endregion

        public bool IsTablet()
        {
            float dpi = Screen.dpi;
            float screenWidth = Screen.width / dpi;
            float screenHeight = Screen.height / dpi;
            double size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            return size >= 6;
        }

        public T isUIObjectAtMouse<T>(Vector2 screenposition)
        {

            RaycastHit2D hit = Physics2D.Raycast(screenposition, Vector2.up);
            if (hit.transform != null)
            {
                var item = hit.transform.gameObject.GetComponent<T>();
                if (item != null)
                    return item;
                else
                {
                    var itemp = hit.transform.gameObject.GetComponentInParent<T>();
                    if (itemp != null)
                        return itemp;
                }
            }
            return default(T);

        }

        public Vector2 Rescale(UI.CanvasScaler scaler)
        {
            return scaler.Rescale(new Vector2(Screen.width, Screen.height));
        }

        public Vector2 Scale(UI.CanvasScaler scaler)
        {
            return scaler.Scale(new Vector2(Screen.width, Screen.height));
        }
    }
}
