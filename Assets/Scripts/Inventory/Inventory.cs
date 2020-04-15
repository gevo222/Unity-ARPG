using sys = System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
