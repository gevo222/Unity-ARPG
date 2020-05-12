using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class ItemInteraction : MonoBehaviour {

    public void OnInteraction(GameObject actor, GameObject target){
		var inv = actor.GetComponent<Inventory>();
		if(inv == null)
			return;

		var itemDef = target.GetComponent<PickUpItem>()?.item;
		Object.Destroy(target);

		inv.Add( new Item(itemDef) );
	}

}

