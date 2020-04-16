using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemManager : MonoBehaviour {

	public static ItemManager main;

	private InventoryRenderer renderer;
	private GameObject        holdingIcon;
	private Item              holdingItem;

	void Start(){
		ItemManager.main = this;
		//inventory.Add( ItemDefs.Katana );
		//inventory.Add( ItemDefs.SmallDagger );
		//inventory.Add( ItemDefs.EpicDagger );
		//inventory.Add( ItemDefs.Cutter2 );
	}

	public void OnInventoryEnter(InventoryRenderer target){
		renderer = target;
	}

	public void OnInventoryExit(InventoryRenderer target){
		if(System.Object.ReferenceEquals(renderer, target)){
			renderer.hover.hidden = true;
			renderer = null;
		}
	}

	public void OnInventoryDown(InventoryRenderer target, PointerEventData evt){
		var inv = target.inventory;

		if(holdingItem == null){
			var gridPos = target.ScreenToGrid(evt.position);
			OccupiedSlot slot = inv.GetItemAt(gridPos);
			if(slot != null){
				inv.Remove(slot);
				PickUp(slot.item);
			}
		} else {
			var hoverPos = target.hover.position;
			OccupiedSlot overlap;
			var result = inv.GetOverlap(hoverPos, holdingItem.size, out overlap);
			switch(result){
			case OverlapType.NONE:
				inv.Add(holdingItem, hoverPos);
				PutDown();
				target.hover.hidden = true;
				break;
			case OverlapType.SINGLE:
				inv.Remove(overlap);
				inv.Add(holdingItem, hoverPos);
				PutDown();
				PickUp(overlap.item);
				break;
			}
		}
	}

	void Update(){
		if(holdingItem == null)
			return;
		var local = ScreenToLocal(Input.mousePosition);
		holdingIcon.transform.localPosition = local;

		if(renderer == null)
			return;

		var gridPos = renderer.ScreenToGrid(Input.mousePosition);
		renderer.hover.position = gridPos;
		var hoverPos = renderer.hover.position; //this value is now clamped. fuck it
		renderer.hover.hidden   = false;
		renderer.hover.size     = holdingItem.size;

		var overlap = renderer.inventory.GetOverlapType(hoverPos, holdingItem.size);
		if(overlap == OverlapType.MULTI){
			renderer.hover.SetErrorColor();
		} else {
			renderer.hover.SetNormalColor();
		}
	}

	private void PickUp(Item item){
		holdingItem = item;
		holdingIcon = item.CreateIcon();
		holdingIcon.transform.SetParent(transform, false);
	}

	private void PutDown(){
		Object.Destroy(holdingIcon);
		holdingItem = null;
		holdingIcon = null;
	}

	private Vector2 ScreenToLocal(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			GetComponent<RectTransform>(), input, Camera.main, out pos
		);
		return pos;
	}
}
