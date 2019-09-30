using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animation))]
    class MenuSlider : MonoBehaviour, IMenuSlider
    {
        private Animation sliderAnimation;
        private RectTransform thisRect;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
            sliderAnimation = GetComponent<Animation>();
        }
        private void Start()
        {
            SetContentSize();
        }

        public void SetContentSize()
        {
            Vector2 scaled = Utilities.ScreenUtilities.Instance.Scale(GetComponentInParent<CanvasScaler>());
            thisRect.sizeDelta = new Vector2(scaled.x*4, 0);
        }
        public void SlideMenuTo(int to)
        {
            sliderAnimation.clip.ClearCurves();
            sliderAnimation.clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.x",
                AnimationCurve.EaseInOut(0, thisRect.anchoredPosition.x, 0.3f, -to * thisRect.sizeDelta.x/4));

            sliderAnimation.Play();
        }
    }
}
