using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultipleDropdown : Dropdown
{
    protected override void Start()
    {
        onValueChanged.RemoveAllListeners();
        onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(int selected)
    {
        
    }
}
