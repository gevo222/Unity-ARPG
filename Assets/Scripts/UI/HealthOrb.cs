using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthOrb : MonoBehaviour {

	private RectTransform rectTransform;

	void Awake(){
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnUpdateHealth(Transform parent, int hp){

		var maxSize = rectTransform.sizeDelta.y - 10.0f;
		var offset = (hp / 100.0f) * maxSize;

		rectTransform.anchoredPosition = new Vector2(0, -offset);
	}
}
