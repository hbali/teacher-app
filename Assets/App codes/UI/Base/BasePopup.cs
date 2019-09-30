using System;
using UnityEngine;

namespace UI.Base
{
    public class BasePopup : MonoBehaviour
    {
        private Animation anim;
        private RectTransform thisRect;
        private bool isOpen;

        Vector2 scaled;

        private static IUIStack _uiStack;

        public static void SetDependencies(IUIStack stack)
        {
            _uiStack = stack;
        }

        protected virtual void Awake()
        {
            AnimationClip animClip = new AnimationClip
            {
                legacy = true
            };
            anim = GetComponent<Animation>();
            anim.playAutomatically = false;
            anim.clip = animClip;
            anim.AddClip(animClip, "animClip");
            thisRect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            SetContentSize();
        }

        private void SetContentSize()
        {
            scaled = Utilities.ScreenUtilities.Instance.Scale(GetComponentInParent<CanvasScaler>());
            thisRect.sizeDelta = scaled - new Vector2(0, 50);
        }

        protected void AnimatePanel(bool on)
        {
            float end = on ? 1 : 0;
            float start = on ? 0 : 1;

            anim.GetClip("animClip").ClearCurves();
            anim.GetClip("animClip").SetCurve("", typeof(RectTransform), "m_Pivot.x",
                AnimationCurve.EaseInOut(0, GetComponent<RectTransform>().pivot.x, 0.3f, end));

            anim.GetClip("animClip").SetCurve("", typeof(CanvasGroup), "m_Alpha",
                AnimationCurve.EaseInOut(0, GetComponent<CanvasGroup>().alpha, 0.3f, end));

            anim.Play("animClip");
        }

        public virtual void OnClose()
        {
            if (isOpen)
            {
                isOpen = false;
                _uiStack.PopupRemoved(this);
                AnimatePanel(false);
            }
        }

        public virtual void OnOpen()
        {
            isOpen = true;
            _uiStack.PopupAdded(this);
            AnimatePanel(true);
        }
    }
}