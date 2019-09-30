using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class SliderWithDisplay : MonoBehaviour
    {
        [SerializeField] private Text number;
        [SerializeField] private Slider slider;

        public int Rating
        {
            get
            {
                return (int)slider.value;
            }
        }

        public void OnValueChanged()
        {
            number.text = slider.value.ToString();
        }
    }
}
