using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo_ColorSquare : MonoBehaviour {
	private Image image;

	void Awake(){
		image = GetComponent<Image> ();
	}
	public void UpdateColorSquare_Red(int index){
		image.color = new Color (index, image.color.g, image.color.b);
	}
	public void UpdateColorSquare_Green(int index){
		image.color = new Color (image.color.r, index, image.color.b);
	}
	public void UpdateColorSquare_Blue(int index){
		image.color = new Color (image.color.r, image.color.g, index);
	}
}
