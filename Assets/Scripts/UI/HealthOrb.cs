using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthOrb : MonoBehaviour {

	private RectTransform rectTransform;

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnUpdateHealth(Transform parent, int hp){

		var maxSize = rectTransform.sizeDelta.x; //assume the orb is circle, use x
		var height = (hp / 100.0f) * maxSize;
		if(height < 10.0f && hp > 0){
			height = 10.0f;
		}

		rectTransform.sizeDelta = new Vector2(maxSize, height);
	}
}
