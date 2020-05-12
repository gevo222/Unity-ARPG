using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class StockInventory : MonoBehaviour {

	[SerializeField]
	private List<ItemObject> stock;

	private Inventory inv;

	void Awake(){

		inv = GetComponent<Inventory>();
		foreach(var item in stock){
			inv.Add( new Item(item) );
		}

	}

}
