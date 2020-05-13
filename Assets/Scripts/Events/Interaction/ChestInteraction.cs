using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class ChestInteraction : MonoBehaviour {

    public void OnInteraction(GameObject actor, GameObject target){
		var lootable = target.GetComponent<Lootable>();
		if(lootable == null)
			return;

		var origin = target.transform.position;
		Object.Destroy(target);

		for(int i = 0; i < lootable.itemCount; i++){
			var itemDef = ItemObject.GetRandomItem();
			ItemSpawner.main.SpawnNear(itemDef, origin);
		}

	}
}

