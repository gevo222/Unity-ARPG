using System.Collections;
using Unity.Collections;
using UnityEngine;


public class Item {

	private ItemObject definition;
	// TODO: things unique to this item? rolls, modifiers, etc.

	public Item(ItemObject definition){
		this.definition = definition;
	}

	public ItemType type => definition.ItemType;
	public Vector2Int size => definition.InvSize;

	public GameObject CreateIcon(Transform parent){
		var obj = Object.Instantiate(definition.UiPrefab, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(parent, false);
		return obj;
	}
}
