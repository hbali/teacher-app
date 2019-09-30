using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Core;
using System.Collections.Generic;

namespace UI.Base
{
    public class GlobalToggle : MonoBehaviour, IGlobalToggleEvents
    {

        #region public fields
        public RectTransform TransformOn;
        public RectTransform TransformOff;
        public List<RectTransform> ExtraImOn;
        public List<RectTransform> ExtraImOff;
        public string Mask = "";
        public bool IsGlobal = false;
        public bool InitialState = false;
        public bool EnableOff = false;
        public bool IsGroup = false;
        #endregion

        #region private fields
        bool state = false;
        #endregion

        #region properties
        public bool State
        {
            get { return state; }
            set { SetActiveInHierarchy(value); }
        }
        #endregion

        #region overrided methods
        protected void Awake()
        {
            state = InitialState;
            EventBus.Instance.register<IGlobalToggleEvents>(this.gameObject);
        }

        protected void OnDestroy()
        {
            EventBus.Instance.unregister(this.gameObject);
        }

        protected void Start()
        {
            SetActiveInHierarchy(InitialState);
        }
        #endregion

        #region user interactions

        /// <summary>
        /// Called when [click].
        /// </summary>
        public void OnClick()
        {
            bool state = false;
            if (!EnableOff)
            {
                state = Mask != "" && IsGlobal ? true : this.TransformOff.gameObject.activeInHierarchy;
            }
            else
                state = !State;
            SetActiveInHierarchy(state);
            if (!IsGroup)
            {
                if (Mask != "" && IsGlobal)
                    EventBus.Instance.post<IGlobalToggleEvents>((e, d) => e.off(this.Mask));
            }
            else
            {
                var group = GetComponentInParent<ToggleGroup>();
                if (group != null)
                {
                    foreach (GlobalToggle gt in group.GetComponentsInChildren<GlobalToggle>())
                    {
                        if (gt != this)
                            gt.SetActiveInHierarchy(false);
                        else if (!group.allowSwitchOff) gt.SetActiveInHierarchy(true);
                    }
                }
            }
        }
        #endregion

        #region public methods
        #endregion

        #region private methods

        void SetActiveInHierarchy(bool state)
        {
            this.state = state;
            this.TransformOff.gameObject.SetActive(!state);
            this.TransformOn.gameObject.SetActive(state);
            if (ExtraImOn != null) this.ExtraImOn.ForEach(x => x.gameObject.SetActive(state));
            if (ExtraImOff != null) this.ExtraImOff.ForEach(x => x.gameObject.SetActive(!state));
        }
        #endregion

        #region GlobalToggleEvents methods
        public void off(string mask)
        {
            if (IsGlobal && mask != this.Mask)
            {
                SetActiveInHierarchy(false);
            }
            else if (IsGlobal && mask == this.Mask && !EnableOff)
            {
                SetActiveInHierarchy(true);
            }
        }

        public void TurnOffAll()
        {
            SetActiveInHierarchy(false);
        }
        #endregion
    }

    public interface IGlobalToggleEvents : IEventSystemHandler
    {
        void TurnOffAll();
        void off(string mask);
    }

}
