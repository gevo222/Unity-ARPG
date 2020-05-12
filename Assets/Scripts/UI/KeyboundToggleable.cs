using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboundToggleable : MonoBehaviour {

	[SerializeField]
	public KeyCode key;
	[SerializeField]
	public GameObject target;

	void Update(){
		if(Input.GetKeyDown(key)){
			var obj = target.transform.gameObject;
			obj.SetActive(!obj.activeSelf);
		}
	}
}
