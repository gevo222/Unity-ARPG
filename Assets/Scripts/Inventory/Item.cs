using System.Collections;
using Unity.Collections;
using UnityEngine;


public class Item {

	private ItemObject definition;
	public ItemType type => definition.ItemType;
	public Vector2Int size => definition.InvSize;


	public Item(ItemObject definition){
		this.definition = definition;
	}

	public GameObject CreateIcon(){
		var obj = Object.Instantiate(definition.UiPrefab, Vector3.zero, Quaternion.identity);
		return obj;
	}
}
