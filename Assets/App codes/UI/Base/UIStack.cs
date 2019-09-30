using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Base
{
    public class UIStack : MonoBehaviour, IUIStack
    {
        private IMenuSlider _slider;
        private IPermanentBottom _permanentBottom;
        private Stack<MenuView> views;
        private Stack<BasePopup> popups;

        private void Awake()
        {
            views = new Stack<MenuView>();
            popups = new Stack<BasePopup>();
        }

        public void SetDependencies(IMenuSlider slider, IPermanentBottom bottom)
        {
            _slider = slider;
            _permanentBottom = bottom;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (popups.Count > 0)
                {
                    popups.Pop().OnClose();
                }
                else if (views.Count > 1)
                {
                    views.Pop();
                    _permanentBottom.CurrentView = views.Pop();
                    _slider?.SlideMenuTo((int)_permanentBottom.CurrentView);
                }
                else
                {
                    //nothing should happen
                }
            }
        }

        public void ViewChanged(MenuView newView)
        {
            views.Push(newView);
        }

        public void PopupAdded(BasePopup popup)
        {
            popups.Push(popup);
        }

        public void PopupRemoved(BasePopup popup)
        {
            if (popups.Count > 0 && popups.Peek() == popup)
            {
                popups.Pop();
            }
        }
    }

    public interface IUIStack
    {
        void SetDependencies(IMenuSlider slider, IPermanentBottom bottom);

        void ViewChanged(MenuView newView);
        void PopupAdded(BasePopup popup);
        void PopupRemoved(BasePopup popup);
    }
}
