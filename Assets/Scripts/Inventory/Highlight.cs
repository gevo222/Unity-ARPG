using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Highlight {

	public GameObject    obj { get; private set; }
	public Image         image;
	public RectTransform rt;

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

	protected InventoryRenderer renderer;
	protected GameObject        obj;
	protected Image             image;
	protected RectTransform     rt;

	public HoverHighlight(InventoryRenderer renderer){
		this.renderer = renderer;
		this.obj      = Object.Instantiate(renderer.HIGHLIGHT_PREFAB, Vector3.zero, Quaternion.identity);
		obj.transform.SetParent(renderer.grid, false);

		this.image    = obj.GetComponent<Image>();
		this.rt       = obj.GetComponent<RectTransform>();
	}

	private Vector2Int _position;
	public  Vector2Int position {
		get { return _position; }
		set {
			_position = renderer.ClampInsideCentered(value, size);
			renderer.PositionOnGrid(rt, _position);
		}
	}
	public Vector2 pixelPosition {
		get { return rt.anchoredPosition; }
	}
	public Vector3 localPosition {
		get { return rt.localPosition; }
	}

	private Vector2Int _size;
	public  Vector2Int size {
		get { return _size; }
		set {
			_size = value;
			renderer.SizeForGrid(rt, value);
		}
	}
	public Vector2 pixelSize {
		get { return rt.sizeDelta; }
	}

	private bool _hidden = false;
	public  bool hidden {
		get { return _hidden; }
		set {
			_hidden = value;
			obj.SetActive(!value);
		}
	}

	public void SetNormalColor(){
		image.color = renderer.HIGHLIGHT_NORMAL;
	}
	public void SetErrorColor(){
		image.color = renderer.HIGHLIGHT_ERROR;
	}
	public void Destroy(){
		Object.Destroy(obj);
	}
}
