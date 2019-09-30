using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo_MultiSelectColorSquare : MonoBehaviour {
	private Image image;
	private bool redFlag, blueFlag, greenFlag;

	void Awake(){
		image = GetComponent<Image> ();
	}

	public void UpdateColorSquare(int index){
		if (index == 0)
			redFlag = !redFlag;
		if (index == 1)
			greenFlag = !greenFlag;
		if (index == 2)
			blueFlag = !blueFlag;
		image.color = new Color ((redFlag ? 1 : 0), (greenFlag ? 1 : 0), (blueFlag ? 1 : 0));
	}
}