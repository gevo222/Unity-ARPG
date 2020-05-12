using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class ItemManager : MonoBehaviour {

	public static ItemManager main;

	private RectTransform     rectTransform;
	private GameObject        tooltipParent;
    private TextMeshProUGUI   tooltipText;

	private GameObject        holdingIcon;
	public  Item              holdingItem;

	void Start(){
		ItemManager.main = this;
		rectTransform = GetComponent<RectTransform>();

		tooltipParent = transform.Find("Tooltip")?.gameObject;
		tooltipText = tooltipParent.transform.Find("Text")?.gameObject.GetComponent<TextMeshProUGUI>();
	}

	void Update(){
		var local = ScreenToLocal(Input.mousePosition);
		tooltipParent.transform.localPosition = local;

		if(holdingItem == null)
			return;

		holdingIcon.transform.localPosition = local;
		if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()){
			ItemSpawner.main.SpawnNearPlayer(holdingItem.definition);
			PutDown();
		}
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

	public void SetTooltip(Item item){
		if(item == null){
			tooltipParent.SetActive(false);
			return;
		}

		var def = item.definition;
		var txt = "<align=center><b>" + def.Name + "</align></b>\n";

		foreach(var val in def.Values){
			var name = val.Key;
			var color = GetAttributeColor(name);
			txt += $"<color={color}><size=20> +{val.Value} {name}</size></color>\n";
		}

		tooltipText.SetText(txt);
		tooltipParent.SetActive(true);
	}

	private string GetAttributeColor(string attribute){
		switch(attribute){
			case "Armor":
				return "#AAAAAA";
			case "Strength":
				return "#CC0000";
			case "Dexterity":
				return "#00CC00";
			case "Intelligence":
				return "#0055FF";
			default:
				return "#FFFFFF";
		}
	}

	private Vector2 ScreenToLocal(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			rectTransform, input, Camera.main, out pos
		);
		return pos;
	}
}
