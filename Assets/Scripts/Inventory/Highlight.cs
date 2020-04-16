using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Highlight {

	public GameObject    obj { get; private set; }
	public Image         image { get; private set; }
	public RectTransform rt { get; private set; }

	public Highlight(GameObject prefab, Transform parent){
		obj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(parent, false);
		image = obj.GetComponent<Image>();
		rt    = obj.GetComponent<RectTransform>();
	}

	public void Destroy(){
		Object.Destroy(obj);
	}
}


public class HoverHighlight {

	private InventoryRenderer renderer;
	private Highlight         highlight;

	public HoverHighlight(InventoryRenderer renderer){
		this.renderer  = renderer;
		this.highlight = new Highlight(renderer.HIGHLIGHT_PREFAB, renderer.grid);
	}

	private Vector2Int _position;
	public  Vector2Int position {
		get { return _position; }
		set {
			_position = renderer.ClampInsideCentered(value, size);
			renderer.PositionOnGrid(highlight.rt, _position);
		}
	}

	private Vector2Int _size;
	public  Vector2Int size {
		get { return _size; }
		set {
			_size = value;
			renderer.SizeForGrid(highlight.rt, value);
		}
	}

	private bool _hidden = false;
	public  bool hidden {
		get { return _hidden; }
		set {
			_hidden = value;
			highlight.obj.SetActive(!value);
		}
	}

	public void UseNormalColor(){
		highlight.image.color = renderer.HIGHLIGHT_NORMAL;
	}

	public void UseErrorColor(){
		highlight.image.color = renderer.HIGHLIGHT_ERROR;
	}

	public void Destroy(){
		highlight.Destroy();
	}
}
