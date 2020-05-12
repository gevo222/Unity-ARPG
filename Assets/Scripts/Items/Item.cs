using System.Collections;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Item {

	public static GameObject ICON_PREFAB;
	static Item(){
		var path = "Assets/Prefabs/UI/ItemIcon.prefab";
		ICON_PREFAB = (GameObject) AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
	}


	public ItemObject definition;
	public Item(ItemObject definition){
		this.definition = definition;
	}
	public ItemType type => definition.ItemType;
	public Vector2Int size => definition.InvSize;


	public GameObject CreateIcon(Transform parent, float scale = 1.0f){

		GameObject obj = null;
		if(definition.UiPrefab != null){
			obj = Object.Instantiate(definition.UiPrefab, parent, false);
			obj.transform.localScale *= scale;
		} else {
			obj = Object.Instantiate(Item.ICON_PREFAB, parent, false);
			obj.GetComponent<Image>().sprite = definition.UiSprite;
			obj.GetComponent<RectTransform>().sizeDelta = new Vector2(
				size.x + (40 * size.x) - 1,
				size.y + (40 * size.y) - 1
			) * scale;
		}

		return obj;
	}
}
