using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InventoryProvider : MonoBehaviour {

	[SerializeField, ReadOnly]
	public int        INV_WIDTH, INV_HEIGHT;
	[SerializeField, ReadOnly]
	public GameObject HIGHLIGHT_PREFAB;

	internal int           GRID_SIZE;
	internal RectTransform grid;

	void Awake(){
		grid      = GetComponent<RectTransform>();
		GRID_SIZE = (int) (grid.rect.width - INV_WIDTH - 1) / INV_WIDTH;
	}

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
			Mathf.FloorToInt(INV_WIDTH * (input.x / grid.rect.width + 0.5f)),
			Mathf.FloorToInt(INV_HEIGHT * (1 - input.y / grid.rect.height))
		);
	}

	public Highlight CreateHighlight(bool centered = false){
		if(centered){
			return new CenteredHighlight(this);
		} else {
			return new Highlight(this);
		}
	}

	public Item AddItem(ItemDef itemDef){
		return new Item(this, itemDef);
	}
}

public class Highlight {

	protected InventoryProvider inv;
	protected GameObject        obj;
	protected Image             image;
	protected RectTransform     rTransform;

	public Highlight(InventoryProvider inv){
		this.inv = inv;
		this.obj = Object.Instantiate(inv.HIGHLIGHT_PREFAB, Vector3.zero, Quaternion.identity);
		this.image = obj.GetComponent<Image>();
		this.rTransform = obj.GetComponent<RectTransform>();
		obj.transform.SetParent(inv.grid, false);
	}

	private Vector2Int _position;
	public  Vector2Int position {
		get { return _position; }
		set {
			if(IsOutOfBounds(value)){
				hidden = true;
			} else {
				hidden = false;
				_position = value = ClampPosition(value);
				rTransform.anchoredPosition = new Vector3(
					 (value.x + (inv.GRID_SIZE * value.x) + 1),
					-(value.y + (inv.GRID_SIZE * value.y) + 1),
					0
				);
			}
		}
	}
	public Vector2 pixelPosition {
		get { return rTransform.anchoredPosition; }
	}
	public Vector3 localPosition {
		get { return rTransform.localPosition; }
	}

	private Vector2Int _size;
	public  Vector2Int size {
		get { return _size; }
		set {
			_size = value;
			rTransform.sizeDelta = new Vector2(
				value.x + (inv.GRID_SIZE * value.x) - 1,
				value.y + (inv.GRID_SIZE * value.y) - 1
			);
		}
	}
	public Vector2 pixelSize {
		get { return rTransform.sizeDelta; }
	}

	private bool _hidden = false;
	public  bool hidden {
		get { return _hidden; }
		set {
			if(value != _hidden){
				_hidden = value;
				obj.SetActive(!value);
			}
		}
	}

	public Color color {
		get { return image.color; }
		set { image.color = value; }
	}

	public void Destroy(){
		Object.Destroy(obj);
	}

	private bool IsOutOfBounds(Vector2Int pos){
		return (
			pos.x < 0 ||
			pos.y < 0 ||
			pos.x >= inv.INV_WIDTH ||
			pos.y >= inv.INV_HEIGHT
		);
	}

	protected virtual Vector2Int ClampPosition(Vector2Int pos){
		return new Vector2Int(
			Mathf.Clamp(pos.x, 0, inv.INV_WIDTH  - size.x),
			Mathf.Clamp(pos.y, 0, inv.INV_HEIGHT - size.y)
		);
	}
}

class CenteredHighlight : Highlight {

	public CenteredHighlight(InventoryProvider inv): base(inv) {}

	protected override Vector2Int ClampPosition(Vector2Int pos){
		return new Vector2Int(
			Mathf.Clamp(pos.x - Mathf.FloorToInt(0.5f * size.x), 0, inv.INV_WIDTH  - size.x),
			Mathf.Clamp(pos.y - Mathf.FloorToInt(0.5f * size.y), 0, inv.INV_HEIGHT - size.y)
		);
	}

}

public class Item {

	private ItemDef    itemDef;
	private Highlight  background;
	private GameObject obj;

	public Item(InventoryProvider inv, ItemDef itemDef){
		this.itemDef    = itemDef;
		this.background = inv.CreateHighlight();
		background.size = itemDef.gridSize;
		background.color = new Color(0, 0, 1.0f, 0.1f);

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
				value.x, value.y, itemDef.position.z
			);
		}
	}

	public Vector2Int position {
		get { return background.position; }
		set { background.position = value; }
	}

	public Vector2Int size {
		get { return itemDef.gridSize; }
	}

	private void AlignItem(){
		obj.transform.localPosition = background.localPosition + itemDef.position;
		obj.transform.localEulerAngles = itemDef.rotation;
	}
}
