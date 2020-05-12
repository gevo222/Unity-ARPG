using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboundDeactivates : MonoBehaviour {

	[SerializeField]
	public KeyCode key;
	[SerializeField]
	public List<GameObject> targets;

	void Update(){
		if(Input.GetKeyDown(key)){
			foreach(var target in targets){
				var obj = target.transform.gameObject;
				obj.SetActive(false);
			}
		}
	}
}
