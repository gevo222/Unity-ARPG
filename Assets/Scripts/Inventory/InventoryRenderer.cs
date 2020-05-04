using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryRenderer :
	MonoBehaviour,
	IPointerDownHandler,
	IPointerEnterHandler,
	IPointerExitHandler
{
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_NORMAL = new Color(1, 1, 1, 0.08f);
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_ERROR  = new Color(1, 0, 0, 0.16f);
	[SerializeField, ReadOnly]
	public Color ITEM_BACKGROUND  = new Color(0, 0, 1.0f, 0.1f);
	[SerializeField]
	public Inventory inventory;

	public RectTransform  grid { get; private set; }
	public GridHighlight  hover { get; private set; }
	public Vector2Int     GRID_SIZE { get; private set; }

	private Dictionary<OccupiedSlot, Highlight> objects;
	private bool inside = false;


	void Start(){
		grid      = GetComponent<RectTransform>();
		hover     = new GridHighlight(this);
		objects   = new Dictionary<OccupiedSlot, Highlight>();
		GRID_SIZE = new Vector2Int(
			(int) (grid.rect.width  - inventory.WIDTH  - 1) / inventory.WIDTH,
			(int) (grid.rect.height - inventory.HEIGHT - 1) / inventory.HEIGHT
		);

		foreach(OccupiedSlot slot in inventory.items){
			AddItem(slot);
		}
		inventory.OnItemAdded.AddListener(AddItem);
		inventory.OnItemRemoved.AddListener(RemoveItem);
	}

	void Update(){
		var held = ItemManager.main.holdingItem;
		if(!inside || held == null)
			return;

		var gridPos = ScreenToGrid(Input.mousePosition);
		hover.position = gridPos;

		var hoverPos = hover.position; //this value is now clamped. fuck it
		hover.hidden = false;
		hover.size   = held.size;

		var overlap = inventory.GetOverlapType(hoverPos, held.size);
		if(overlap == OverlapType.MULTI)
			hover.UseErrorColor();
		else
			hover.UseNormalColor();
	}

	public void OnPointerDown(PointerEventData evt){
		var held = ItemManager.main.holdingItem;

		if(held == null){
			var gridPos = ScreenToGrid(evt.position);
			OccupiedSlot slot = inventory.GetItemAt(gridPos);
			if(slot != null){
				inventory.Remove(slot);
				ItemManager.main.PickUp(slot.item);
			}
		} else {
			var hoverPos = hover.position;
			OccupiedSlot overlap;
			var result = inventory.GetOverlap(hoverPos, held.size, out overlap);
			switch(result){
			case OverlapType.NONE:
				inventory.Add(held, hoverPos);
				ItemManager.main.PutDown();
				hover.hidden = true;
				break;
			case OverlapType.SINGLE:
				inventory.Remove(overlap);
				inventory.Add(held, hoverPos);
				ItemManager.main.PutDown();
				ItemManager.main.PickUp(overlap.item);
				break;
			}
		}
	}

	public void OnPointerEnter(PointerEventData evt){
		inside = true;
	}

	public void OnPointerExit(PointerEventData evt){
		inside = false;
		hover.hidden = true;
	}

	/* POSITION TRANSFORMATIONS */
	public Vector2 ScreenToLocal(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			grid, input, Camera.main, out pos
		);
		return pos;
	}
	public Vector2Int ScreenToGrid(Vector2 input){
		Vector2 local = ScreenToLocal(input);
		return LocalToGrid(local);
	}
	public Vector2Int LocalToGrid(Vector2 input){
		return new Vector2Int(
			Mathf.FloorToInt(inventory.WIDTH * (input.x / grid.rect.width + 0.5f)),
			Mathf.FloorToInt(inventory.HEIGHT * (1 - input.y / grid.rect.height))
		);
	}

	/* POSITION/SIZE ALTERING */
	public Vector2 GridToRealPosition(Vector2Int pos){
		return new Vector2(
			 (pos.x + (GRID_SIZE.x * pos.x) + 1),
			-(pos.y + (GRID_SIZE.y * pos.y) + 1)
		);
	}
	public Vector2 GridToRealSize(Vector2Int size){
		var x = Mathf.Min(size.x, inventory.WIDTH);
		var y = Mathf.Min(size.y, inventory.HEIGHT);
		return new Vector2(
			x + (GRID_SIZE.x * x) - 1,
			y + (GRID_SIZE.y * y) - 1
		);
	}
	public Vector2Int ClampInside(Vector2Int pos, Vector2Int size){
		return new Vector2Int(
			Mathf.Clamp(pos.x, 0, inventory.WIDTH  - size.x),
			Mathf.Clamp(pos.y, 0, inventory.HEIGHT - size.y)
		);
	}
	public Vector2Int ClampInsideCentered(Vector2Int pos, Vector2Int size){
		return new Vector2Int(
			Mathf.Clamp(pos.x - Mathf.FloorToInt(0.5f * size.x), 0, inventory.WIDTH  - size.x),
			Mathf.Clamp(pos.y - Mathf.FloorToInt(0.5f * size.y), 0, inventory.HEIGHT - size.y)
		);
	}


	/* INTERNAL METHODS */
	private void AddItem(OccupiedSlot slot){
		var highlight = new Highlight(grid) {
			color = ITEM_BACKGROUND,
			position = GridToRealPosition(slot.position),
			size = GridToRealSize(slot.item.size)
		};

		var obj = slot.item.CreateIcon(highlight.transform);
		obj.transform.localPosition = new Vector3(
			highlight.size.x / 2, -highlight.size.y / 2, 0
		);

		objects.Add(slot, highlight);
	}

	private void RemoveItem(OccupiedSlot slot){
		Highlight obj;
		if(objects.TryGetValue(slot, out obj)){
			obj.Destroy();
			objects.Remove(slot);
		}
	}
}
