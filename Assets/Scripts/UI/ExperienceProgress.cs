using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExperienceProgress : MonoBehaviour {

	private RectTransform rectTransform;

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetPercent(float percent){
		rectTransform.sizeDelta = new Vector2(800 * percent, 12);
	}
}
