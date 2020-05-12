using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;


public class ItemEvent : UnityEvent<OccupiedSlot> { }

public enum OverlapType {
	NONE,
	SINGLE,
	MULTI
}

public class Inventory : MonoBehaviour {

	[SerializeField, ReadOnly]
	public int WIDTH = 12, HEIGHT = 5;

	public ItemEvent OnItemAdded { get; private set; }
	public ItemEvent OnItemRemoved { get; private set; }
	public List<OccupiedSlot> items { get; private set; }

	private bool[,] grid;

	private Inventory(){
		grid          = new bool[WIDTH, HEIGHT];
		items         = new List<OccupiedSlot>();
		OnItemAdded   = new ItemEvent();
		OnItemRemoved = new ItemEvent();
	}

	public OccupiedSlot Add(Item item){
		var position = FirstOpenSlotFor(item.size);
		if(position == null){
			throw new System.Exception("No space for item.");
		}
		return Add(item, (Vector2Int) position);
	}

	public OccupiedSlot Add(Item item, Vector2Int position){
		if(!SlotFits(position, item.size))
			throw new System.Exception("Invalid item position.");

		var slot = new OccupiedSlot(item, position);
		items.Add(slot);
		RecalculateGrid();
		OnItemAdded.Invoke(slot);
		return slot;
	}

	public void Remove(OccupiedSlot slot){
		items.Remove(slot);
		RecalculateGrid();
		OnItemRemoved.Invoke(slot);
	}

	public OccupiedSlot GetItemAt(Vector2Int gridPos){
		foreach(OccupiedSlot slot in items){
			if(slot.ContainsPoint(gridPos)){
				return slot;
			}
		}
		return null;
	}

	public Vector2Int? FirstOpenSlotFor(Vector2Int size){
		for(int px = 0; px < WIDTH; px++){
			for(int py = 0; py < HEIGHT; py++){
				var pos = new Vector2Int(px, py);
				if(SlotFits(pos, size))
					return pos;
			}
		}
		return null;
	}

	public bool SlotFits(Vector2Int pos, Vector2Int size){
		for(int ix = 0; ix < size.x; ix++){
			for(int iy = 0; iy < size.y; iy++){
				try {
					if(grid[pos.x + ix, pos.y + iy]) //slot is occupied
						return false;
				} catch(System.IndexOutOfRangeException){ //or out of bounds
					return false;
				}
			}
		}
		return true;
	}

	public OverlapType GetOverlapType(Vector2Int pos, Vector2Int size){
		OccupiedSlot overlap;
		return GetOverlap(pos, size, out overlap);
	}

	public OverlapType GetOverlap(Vector2Int pos, Vector2Int size, out OccupiedSlot overlap){
		OccupiedSlot last = null;
		int count = 0;
		foreach(OccupiedSlot slot in items){
			if(slot.Overlaps(pos, size)){
				count += 1;
				last = slot;
			}
		}
		switch(count){
			case 0:
				overlap = null;
				return OverlapType.NONE;
			case 1:
				overlap = last;
				return OverlapType.SINGLE;
			default:
				overlap = null;
				return OverlapType.MULTI;
		}
	}

	private void RecalculateGrid(){
		grid = new bool[WIDTH, HEIGHT];
		foreach(OccupiedSlot slot in items){
			var pos = slot.position;
			var size = slot.item.size;
			for(int x = pos.x; x < pos.x + size.x; x++){
				for(int y = pos.y; y < pos.y + size.y; y++){
					grid[x,y] = true;
				}
			}
		}
	}

}

public class OccupiedSlot {

	public Item       item { get; private set; }
	public Vector2Int position { get; private set; }

	public OccupiedSlot(Item item, Vector2Int position){
		this.item     = item;
		this.position = position;
	}

	public bool ContainsPoint(Vector2Int test){
		return (
			test.x >= position.x &&
			test.y >= position.y &&
			test.x < (position.x + item.size.x) &&
			test.y < (position.y + item.size.y)
		);
	}

	public bool Overlaps(Vector2Int tpos, Vector2Int tsize){
		return (
			position.x < tpos.x + tsize.x &&
			position.x + item.size.x > tpos.x &&
			position.y < tpos.y + tsize.y &&
			position.y + item.size.y > tpos.y
		);
	}
}
