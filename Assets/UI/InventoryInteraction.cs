using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryInteraction :
	MonoBehaviour,
	IPointerDownHandler
{
	private InventoryProvider inv;
	private Item dagger;

	private Highlight hover;
	private Item      holdingItem;


	void Start(){
		inv    = GetComponent<InventoryProvider>();
		hover  = inv.CreateHighlight(centered: true);
		dagger = inv.AddItem(ItemDefs.Katana);
	}

	void Update(){
		if(holdingItem == null)
			return;

		var local   = inv.ScreenToLocal(Input.mousePosition);
		var gridPos = inv.LocalToGrid(local);
		hover.position = gridPos;

		holdingItem.itemPosition = local;
	}

	public void OnPointerDown(PointerEventData evt){
		var gridPos = inv.ScreenToGrid(evt.position);
		if(holdingItem == null){
			PickUp(dagger);
		} else {
			PutDown(holdingItem);
		}
	}

	private void PickUp(Item item){
		holdingItem   = item;
		hover.size    = item.size;
		hover.hidden  = false;
		item.detached = true;
	}

	private void PutDown(Item item){
		hover.hidden  = true;
		item.position = hover.position;
		item.detached = false;
		holdingItem   = null;
	}

}

