using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Elements
{
    public class SettingsToggle : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private UnityEvent StateChange;
        public bool state;

        public void SetState(bool state)
        {
            this.state = state;
            animator.SetBool("click_yes", !state);
        }

        public void OnClick()
        {
            state = !state;
            animator.SetBool("click_yes", !state);
            StateChange.Invoke();
        }
    }
}
