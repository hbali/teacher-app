using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    class SplashScreen : MonoBehaviour 
    {
        [SerializeField] private Animator animator;
        private void Awake()
        {
            SceneManager.LoadSceneAsync("main");
            animator = GetComponent<Animator>();
            animator.SetTrigger("on");
        }

        public void FirstAnimDone()
        {
            animator.SetTrigger("on2");
        }
    }
}
