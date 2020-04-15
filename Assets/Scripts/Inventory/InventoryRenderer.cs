using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;


public class InventoryRenderer : MonoBehaviour {

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
