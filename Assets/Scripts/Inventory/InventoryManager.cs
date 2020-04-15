using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventoryManager :
	MonoBehaviour,
	IPointerDownHandler
{
	private static Color HIGHLIGHT_RED    = new Color(1, 0, 0, 0.16f);
	private static Color HIGHLIGHT_NORMAL = new Color(1, 1, 1, 0.08f);

	private Inventory         inventory;
	private InventoryRenderer invRenderer;

	private Item      holdingItem;
	private Highlight hover;


	void Start(){
		invRenderer = GetComponent<InventoryRenderer>();
		inventory   = new Inventory(invRenderer);
		hover       = invRenderer.CreateHighlight(centered: true);

		inventory.Add( ItemDefs.Katana );
		inventory.Add( ItemDefs.SmallDagger );
		inventory.Add( ItemDefs.EpicDagger );
		inventory.Add( ItemDefs.Cutter2 );
	}

	void Update(){
		if(holdingItem == null)
			return;

		var local = invRenderer.ScreenToLocal(Input.mousePosition);
		holdingItem.itemPosition = local;

		var gridPos = invRenderer.LocalToGrid(local);
		hover.position = gridPos;

		if(OverlappingItems() >= 2){
			hover.color = HIGHLIGHT_RED;
		} else {
			hover.color = HIGHLIGHT_NORMAL;
		}
	}

	public void OnPointerDown(PointerEventData evt){
		var gridPos = invRenderer.ScreenToGrid(evt.position);
		if(holdingItem == null){
			Item item = inventory.GetItemAt(gridPos);
			if(item != null){
				PickUp((Item) item);
			}
		} else {
			Item overlap;
			int overlappingItems = OverlappingItems(out overlap);
			if(overlappingItems <= 1)
				PutDown();
			if(overlappingItems == 1)
				PickUp(overlap);
		}
	}

	private int OverlappingItems(){
		Item dummy;
		return OverlappingItems(out dummy);
	}

	private int OverlappingItems(out Item overlap){
		Item lastItem = null;
		int result = 0;
		if(holdingItem != null){
			foreach(Item item in inventory){
				if(!Object.ReferenceEquals(holdingItem, item) && item.Overlaps(hover)){
					result += 1;
					lastItem = item;
				}
			}
		}
		overlap = lastItem;
		return result;
	}

	private void PickUp(Item item){
		holdingItem   = item;
		hover.size    = item.size;
		hover.hidden  = false;
		item.detached = true;
	}

	private void PutDown(){
		hover.hidden = true;
		holdingItem.position = hover.position;
		holdingItem.detached = false;
		holdingItem  = null;
	}
}
