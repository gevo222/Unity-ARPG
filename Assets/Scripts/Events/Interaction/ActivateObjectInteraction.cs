using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class ActivateObjectInteraction : MonoBehaviour {

    public void OnInteraction(GameObject actor, GameObject target){

		var objs = target.GetComponent<ActivateObject>()?.targets;
		if(objs == null){
			return;
		}

		foreach(var obj in objs){
			obj.SetActive(true);
		}

	}
}

