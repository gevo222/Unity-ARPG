using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryInteraction :
	MonoBehaviour,
	IPointerClickHandler
{
	[SerializeField, ReadOnly]
	public int        INV_WIDTH, INV_HEIGHT;
	[SerializeField, ReadOnly]
	public GameObject HIGHLIGHT_PREFAB;

	private int             GRID_SIZE;
	private RectTransform   grid;
	private Highlight       hover;
	private List<Highlight> backgrounds;

	void Start(){
		grid       = GetComponent<RectTransform>();
		GRID_SIZE  = (int) (grid.rect.width - (INV_WIDTH + 1)) / INV_WIDTH;
		hover      = new Highlight(this);

		hover.color = new Color(1.0f, 0, 0, 0.08f);
		hover.size = new Vector2Int(2,4);
	}

	void Update(){
		var gridPos = ToGrid(Input.mousePosition);
		hover.position = gridPos;
	}

	public void OnPointerClick(PointerEventData data){
		var gridPos = ToGrid(data.position);
	}

	private Vector2 Localize(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			grid, input, Camera.main, out pos
		);
		return pos;
	}

	private Vector2Int ToGrid(Vector2 input){
		Vector2 local = Localize(input);
		var x = grid.rect.width/2 + local.x;
		var y = grid.rect.height  - local.y;
		var gridX = Mathf.FloorToInt(x / grid.rect.width * INV_WIDTH);
		var gridY = Mathf.FloorToInt(y / grid.rect.height * INV_HEIGHT);
		return new Vector2Int(gridX, gridY);
	}


	class Highlight {

		private InventoryInteraction inv;
		private GameObject           obj;
		private Image                image;
		private RectTransform        rectTransform;

		public Highlight(InventoryInteraction inv){
			this.inv = inv;
			this.obj = Object.Instantiate(inv.HIGHLIGHT_PREFAB, Vector3.zero, Quaternion.identity);
			this.image = obj.GetComponent<Image>();
			this.rectTransform = obj.GetComponent<RectTransform>();
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
					value = _position = new Vector2Int(
						System.Math.Min(value.x, inv.INV_WIDTH - size.x),
						System.Math.Min(value.y, inv.INV_HEIGHT - size.y)
					);
					rectTransform.anchoredPosition = new Vector3(
						 (value.x + (inv.GRID_SIZE * value.x) + 1),
						-(value.y + (inv.GRID_SIZE * value.y) + 1),
						0
					);
				}
			}
		}

		private Vector2Int _size;
		public  Vector2Int size {
			get { return _size; }
			set {
				_size = value;
				rectTransform.sizeDelta = new Vector2(
					value.x + (inv.GRID_SIZE * value.x) - 1,
					value.y + (inv.GRID_SIZE * value.y) - 1
				);
			}
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
	}
}
