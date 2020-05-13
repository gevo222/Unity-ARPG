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

		var itemPool = ItemObject.All.Values.ToList();
		for(int i = 0; i < lootable.itemCount; i++){
			var index = Random.Range(0, itemPool.Count);
			var itemDef = itemPool[index];
			ItemSpawner.main.SpawnNear(itemDef, origin);
			itemPool.RemoveAt(index);
		}

	}
}

