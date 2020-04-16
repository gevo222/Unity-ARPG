using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Highlight {

	private GameObject    obj;
	private Image         image;
	private RectTransform rt;

	public Highlight(GameObject prefab, Transform parent){
		obj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(parent, false);
		image = obj.GetComponent<Image>();
		rt    = obj.GetComponent<RectTransform>();
	}

	public Transform transform => obj.transform;
	public Vector2 position {
		get { return rt.anchoredPosition; }
		set { rt.anchoredPosition = value; }
	}
	public Vector2 size {
		get { return rt.sizeDelta; }
		set { rt.sizeDelta = value; }
	}
	public Color color {
		get { return image.color; }
		set { image.color = value; }
	}
	private bool _hidden;
	public bool hidden {
		get { return _hidden; }
		set {
			obj.SetActive(!value);
			_hidden = value;
		}
	}

	public void Destroy(){
		Object.Destroy(obj);
	}
}


public class GridHighlight {

	private InventoryRenderer renderer;
	private Highlight         highlight;

	public GridHighlight(InventoryRenderer renderer){
		this.renderer  = renderer;
		this.highlight = new Highlight(renderer.HIGHLIGHT_PREFAB, renderer.grid);
	}

	private Vector2Int _position;
	public  Vector2Int position {
		get { return _position; }
		set {
			_position = renderer.ClampInsideCentered(value, size);
			highlight.position = renderer.GridToRealPosition(_position);
		}
	}

	private Vector2Int _size;
	public  Vector2Int size {
		get { return _size; }
		set {
			_size = value;
			highlight.size = renderer.GridToRealSize(value);
		}
	}

	public bool hidden {
		get { return highlight.hidden; }
		set { highlight.hidden = value; }
	}

	public void UseNormalColor(){
		highlight.color = renderer.HIGHLIGHT_NORMAL;
	}

	public void UseErrorColor(){
		highlight.color = renderer.HIGHLIGHT_ERROR;
	}

	public void Destroy(){
		highlight.Destroy();
	}
}
