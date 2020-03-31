using sys = System;
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

class Inventory : List<Item> {

	private InventoryRenderer invRenderer;
	private bool[,] grid;

	public Inventory(InventoryRenderer invRenderer){
		this.invRenderer = invRenderer;
	}

	public new void Add(Item item){}

	public void Add(ItemDef itemDef){
		var pos = FirstOpenSlotFor(itemDef);
		if(pos != null){
			var item = invRenderer.AddItem(itemDef);
			item.position = (Vector2Int) pos;
			base.Add(item);
		}
	}

	public Vector2Int? FirstOpenSlotFor(ItemDef itemDef){
		RecalculateGrid();
		for(int px = 0; px < invRenderer.INV_WIDTH; px++){
			for(int py = 0; py < invRenderer.INV_HEIGHT; py++){
				if(ItemFitsInSlot(itemDef, px, py))
					return new Vector2Int(px, py);
			}
		}
		return null;
	}

	public Item GetItemAt(Vector2Int gridPos){
		foreach(Item item in this){
			if(item.ContainsPoint(gridPos)){
				return item;
			}
		}
		return null;
	}

	private void RecalculateGrid(){
		grid = new bool[invRenderer.INV_WIDTH,invRenderer.INV_HEIGHT];
		foreach(Item item in this){
			for(int x = item.position.x; x < item.position.x + item.size.x; x++){
				for(int y = item.position.y; y < item.position.y + item.size.y; y++){
					grid[x,y] = true;
				}
			}
		}
	}


	private bool ItemFitsInSlot(ItemDef itemDef, int px, int py){
		var size = itemDef.gridSize;
		for(int ix = 0; ix < size.x; ix++){
			for(int iy = 0; iy < size.y; iy++){
				try {
					if(grid[px + ix, py + iy]) //slot is occupied
						return false;
				} catch(sys.IndexOutOfRangeException e){ //or out of bounds
					return false;
				}
			}
		}
		return true;
	}
}
