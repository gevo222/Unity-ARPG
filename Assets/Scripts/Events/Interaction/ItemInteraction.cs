using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class ItemInteraction : MonoBehaviour {

    public void OnInteraction(GameObject actor, GameObject target){
		var itemDef = target.GetComponent<PickUpItem>()?.item;
		if(itemDef == null)
			return;

		Debug.Log($"Player picked up: {target.name}");
		var inv = actor.GetComponent<Inventory>();
		if(inv != null){
			Object.Destroy(target);
			inv.Add( new Item(itemDef) );
		}

	}
}

