using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Item {

	private static Color BACKGROUND_COLOR = new Color(0, 0, 1.0f, 0.1f);

	private ItemDef    itemDef;
	private Highlight  background;
	private GameObject obj;

	public Item(InventoryRenderer inv, ItemDef itemDef){
		this.itemDef    = itemDef;
		this.background = inv.CreateHighlight();
		background.size = itemDef.gridSize;
		background.color = BACKGROUND_COLOR;

		this.obj = Object.Instantiate(itemDef.resource, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(inv.grid, false);
		obj.transform.localScale = itemDef.scale;
		AlignItem();
	}

	private bool _detached;
	public  bool detached {
		get { return _detached; }
		set {
			_detached = value;
			background.hidden = value;
			if(!value){
				AlignItem();
			}
		}
	}

	public Vector2 itemPosition {
		get { return obj.transform.localPosition; }
		set {
			if(!detached) return;
			obj.transform.localPosition = new Vector3(
				value.x, value.y, itemDef.position.z - 10
			);
		}
	}

	public Vector2Int position {
		get { return background.position; }
		set {
			background.position = value;
			AlignItem();
		}
	}

	public Vector2Int size {
		get { return itemDef.gridSize; }
	}

	public bool ContainsPoint(Vector2Int test){
		return (
			test.x >= position.x &&
			test.y >= position.y &&
			test.x < (position.x + size.x) &&
			test.y < (position.y + size.y)
		);
	}

	public bool Overlaps(Highlight target){
		return (
			position.x < target.position.x + target.size.x &&
			position.x + size.x > target.position.x &&
			position.y < target.position.y + target.size.y &&
			position.y + size.y > target.position.y
		);
	}

	public void Destroy(){
		background.Destroy();
		Object.Destroy(obj);
	}

	private void AlignItem(){
		obj.transform.localPosition = background.localPosition + itemDef.position;
		obj.transform.localEulerAngles = itemDef.rotation;
	}
}
