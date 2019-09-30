using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Base
{
    class InfoHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform textPanel;

        public void OnClick()
        {
            textPanel.gameObject.SetActive(!textPanel.gameObject.activeSelf);
        }
    }
}
