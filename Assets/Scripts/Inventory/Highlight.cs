using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Highlight {

	protected InventoryRenderer inv;
	protected GameObject        obj;
	protected Image             image;
	protected RectTransform     rTransform;

	public Highlight(InventoryRenderer inv){
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

	public CenteredHighlight(InventoryRenderer inv): base(inv) {}

	protected override Vector2Int ClampPosition(Vector2Int pos){
		return new Vector2Int(
			Mathf.Clamp(pos.x - Mathf.FloorToInt(0.5f * size.x), 0, inv.INV_WIDTH  - size.x),
			Mathf.Clamp(pos.y - Mathf.FloorToInt(0.5f * size.y), 0, inv.INV_HEIGHT - size.y)
		);
	}

}
