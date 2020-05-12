using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class SingleSlot :
	MonoBehaviour,
	IPointerDownHandler,
	IPointerEnterHandler,
	IPointerExitHandler
{
	[SerializeField, ReadOnly]
	public int WIDTH = 1, HEIGHT = 1;
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_NORMAL = new Color(1, 1, 1, 0.08f);
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_ERROR  = new Color(1, 0, 0, 0.16f);
	[SerializeField, ReadOnly]
	public Color ITEM_BACKGROUND  = new Color(0, 0, 1.0f, 0.1f);
	[SerializeField, ReadOnly]
	public ItemType ALLOW_TYPE;

	public Highlight background { get; private set; }

	void Awake(){
		var size = GetComponent<RectTransform>().sizeDelta;
		background = new Highlight(transform) {
			size = new Vector2(size.x - 2, size.y - 2),
			position = new Vector2(1, -1),
			hidden = true
		};
	}

	public void OnPointerDown(PointerEventData evt){
		var held = ItemManager.main.holdingItem;

		if(held == null && item != null){
			ItemManager.main.PickUp(item);
			item = null;
			background.color = HIGHLIGHT_NORMAL;
			return;
		}

		if(held != null && CanHoldItem(held)){
			ItemManager.main.PutDown();
			if(item != null)
				ItemManager.main.PickUp(item);
			item = held;
			background.color = ITEM_BACKGROUND;
		}

		ItemManager.main.SetTooltip(item);
	}

	public void OnPointerEnter(PointerEventData evt){
		ItemManager.main.SetTooltip(item);

		var held = ItemManager.main.holdingItem;
		if(held != null){
			background.hidden = false;
			background.color = CanHoldItem(held) ? HIGHLIGHT_NORMAL : HIGHLIGHT_ERROR;
		}
	}

	public void OnPointerExit(PointerEventData evt){
		background.hidden = IsEmpty();
		background.color = ITEM_BACKGROUND;
		ItemManager.main.SetTooltip(null);
	}


	private GameObject itemIcon;
	private Item _item;
	public Item item {
		get { return _item; }
		set {
			if(value != null && !CanHoldItem(value))
				throw new System.Exception("Slot cannot hold this item.");
			if(itemIcon != null)
				Object.Destroy(itemIcon);
			if(value != null)
				itemIcon = value.CreateIcon(transform);
			_item = value;
		}
	}

	public bool CanHoldItem(Item item){
		return (
			item.size.x <= WIDTH &&
			item.size.y <= HEIGHT &&
			item.type == ALLOW_TYPE
	   	);
	}

	public bool IsEmpty(){
		return item == null;
	}
}
