using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemManager : MonoBehaviour {

	public static ItemManager main;

	private RectTransform     rectTransform;
	private GameObject        holdingIcon;
	public  Item              holdingItem;

	void Start(){
		ItemManager.main = this;
		rectTransform = GetComponent<RectTransform>();
	}

	void Update(){
		if(holdingItem == null)
			return;

		var local = ScreenToLocal(Input.mousePosition);
		holdingIcon.transform.localPosition = local;
	}

	public void PickUp(Item item){
		holdingItem = item;
		holdingIcon = item.CreateIcon(transform);
	}

	public void PutDown(){
		Object.Destroy(holdingIcon);
		holdingItem = null;
		holdingIcon = null;
	}

	private Vector2 ScreenToLocal(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			rectTransform, input, Camera.main, out pos
		);
		return pos;
	}
}
