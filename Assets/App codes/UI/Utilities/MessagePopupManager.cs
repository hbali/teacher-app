using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Utilities
{
    class MessagePopupManager : MonoBehaviour
    {
        [SerializeField]
        Image leftImg;
        [SerializeField]
        Text upperText;
        [SerializeField]
        Text leftButtonText;
        [SerializeField]
        Text rightButtonText;

        [SerializeField]
        RectTransform leftButtonRect;
        [SerializeField]
        RectTransform rightButtonRect;
        [SerializeField]
        RectTransform mainPanelRect;

        [SerializeField]
        RectTransform blocker;

        [SerializeField] private RectTransform loader;

        private Action leftButtonAction;
        private Action rightButtonAction;
        private bool onlyRight;

        void Awake()
        {
            instance = this;
        }

        private static MessagePopupManager instance;

        public static MessagePopupManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void StartLoader(string text)
        {
            loader.gameObject.SetActive(true);
        }

        public void StopLoader()
        {
            loader.gameObject.SetActive(false);
        }

        public MessagePopupManager SetLeftImg(Sprite sprite)
        {
            leftImg.sprite = sprite;
            return instance;

        }
        public MessagePopupManager SetUpperText(string txt)
        {
            upperText.text = txt;
            return instance;

        }

        public MessagePopupManager SetLeftButtonText(string txt)
        {
            leftButtonText.text = txt;
            return instance;

        }
        public MessagePopupManager SetRightButtonText(string txt)
        {
            rightButtonText.text = txt;
            return instance;

        }

        public MessagePopupManager OnlyRightButton(bool onlyRight)
        {
            this.onlyRight = onlyRight;
            return instance;
        }

        /// <summary>
        /// If Action parameter is null, the default action will be set which only disables the popup
        /// </summary>
        /// <param name="act"></param>
        public MessagePopupManager SetLeftButtonAction(Action act = null)
        {
            leftButtonAction = act ?? DisablePopup;
            return instance;
        }

        /// <summary>
        /// If Action parameter is null, the default action will be set which only disables the popup
        /// </summary>
        /// <param name="act"></param>
        public MessagePopupManager SetRightButtonAction(Action act = null)
        {
            rightButtonAction = act ?? DisablePopup;
            return instance;
        }

        public void OnRightButton()
        {
            if (rightButtonAction != null)
            {
                rightButtonAction.Invoke();
            }
            if (rightButtonAction != DisablePopup)
            {
                DisablePopup();
            }
        }

        public void OnLeftButton()
        {
            if (leftButtonAction != null)
            {
                leftButtonAction.Invoke();
            }

            if (leftButtonAction != DisablePopup)
            {
                DisablePopup();
            }
        }

        /// <summary>
        /// In this case the left and right button wont be displayed, as the popup disappears after a specific amount of time
        /// </summary>
        /// <param name="time">time in seconds to disappear</param>
        public void EnablePopup(float time)
        {
            blocker.gameObject.SetActive(true);
            mainPanelRect.gameObject.SetActive(true);
            rightButtonRect.gameObject.SetActive(false);
            Invoke("DisablePopup", time);
        }

        /// <summary>
        /// Enables the popup with left and right button (so set the texts before)
        /// </summary>
        public void EnablePopup()
        {
            blocker.gameObject.SetActive(true);
            mainPanelRect.gameObject.SetActive(true);
            leftButtonRect.gameObject.SetActive(!onlyRight);
        }

        public void DisablePopup()
        {
            CancelInvoke("DisablePopup");
            blocker.gameObject.SetActive(false);
            mainPanelRect.gameObject.SetActive(false);
            upperText.text = "";
            rightButtonText.text = "";
            leftButtonText.text = "";
            leftButtonAction = null;
            rightButtonAction = null;
        }
    }
}
