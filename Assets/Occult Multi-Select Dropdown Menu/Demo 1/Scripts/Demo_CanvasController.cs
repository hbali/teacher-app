using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_CanvasController : MonoBehaviour {
	private const int DISP_LIVES = 1, DISP_HEALTH = 2, DISP_LEVELNAME = 3;
	[SerializeField] private Occult.UI.MultiSelectDropdown dropdown;
	[SerializeField] private UnityEngine.UI.Text displayChoiceText;
	[SerializeField] private CanvasGroup livesCGroup, healthCGroup, levelNameCGroup;

	void Awake(){
		foreach (int value in dropdown.value) {
			OnDropdownChange (value);
		}
		UpdateSelectionText ();
	}

	private void UpdateSelectionText(){
		displayChoiceText.text = "Current Selections:\n";
		for (int i = 0; i < dropdown.value.Count; i++) {
			if (dropdown.value [i] == DISP_LIVES)
				displayChoiceText.text += "Displaying Lives \n";
			if (dropdown.value [i] == DISP_HEALTH)
				displayChoiceText.text += "Displaying Health \n";
			if (dropdown.value [i] == DISP_LEVELNAME)
				displayChoiceText.text += "Displaying Level Name \n";
		}
		if(displayChoiceText.text.Length < 25)
			displayChoiceText.text += "None\n";
	}

	public void OnDropdownChange(int val){
		livesCGroup.alpha = 0;
		healthCGroup.alpha = 0;
		levelNameCGroup.alpha = 0;

		for (int i = 0; i < dropdown.value.Count; i++) {
			if (dropdown.value [i] == DISP_LIVES)
				livesCGroup.alpha = (livesCGroup.alpha > 0 ? 0 : 1);
			if (dropdown.value [i] == DISP_HEALTH)
				healthCGroup.alpha = (healthCGroup.alpha > 0 ? 0 : 1);
			if (dropdown.value [i] == DISP_LEVELNAME)
				levelNameCGroup.alpha = (levelNameCGroup.alpha > 0 ? 0 : 1);
		}
		UpdateSelectionText ();
	}
}