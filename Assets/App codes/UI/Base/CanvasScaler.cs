using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class CanvasScaler : UIBehaviour
    {
        private UnityEngine.UI.CanvasScaler scaler
        {
            get { return GetComponent<UnityEngine.UI.CanvasScaler>() ?? GetComponentInParent<UnityEngine.UI.CanvasScaler>(); }
        }

        protected override void Awake()
        {
            base.Start();
            float sf = Screen.dpi < 150 ? 1f : Screen.dpi / 150;
            scaler.scaleFactor = sf;
        }

        /// <summary>
        /// Scales the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Vector2 Scale(Vector2 value)
        {
            var factor = scaler.scaleFactor;
            var result = new Vector2(value.x, value.y);
            result.Scale(new Vector2(1 / factor, 1 / factor));
            return result;
        }

        /// <summary>
        /// Rescales the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Vector2 Rescale(Vector2 value)
        {
            var factor = scaler.scaleFactor;
            var result = new Vector2(value.x, value.y);
            result.Scale(new Vector2(factor, factor));
            return result;
        }

        /// <summary>
        /// Rescales the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public float Rescale(float value)
        {
            var factor = scaler.scaleFactor;
            var result = new Vector2(value, 0);
            result.Scale(new Vector2(factor, factor));
            return result.x;
        }
    }
}

