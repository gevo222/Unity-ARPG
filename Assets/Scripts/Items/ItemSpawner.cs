using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class ItemSpawner : MonoBehaviour {

	public static ItemSpawner main;

	[SerializeField]
	public GameObject parent;
	[SerializeField]
	public Shader hoverShader;
	[SerializeField]
	public float itemScaleFactor = 1.2f;


	void Start(){
		ItemSpawner.main = this;
		//Spawn(ItemObject.All["Items/Weapons/Cutter"], Vector3.zero);
		//Spawn(ItemObject.All["Items/Weapons/Epic_Dagger"], Vector3.zero);
	}

	public GameObject Spawn(ItemObject item, Vector3 position){
		var obj = Object.Instantiate(item.WorldPrefab, position, Quaternion.identity);

		obj.transform.SetParent(parent.transform, false);
		obj.transform.localScale *= itemScaleFactor;
		obj.transform.localEulerAngles += new Vector3(
			0, Random.Range(0,360), Random.Range(0,360)
		);

		var rigidbody = obj.AddComponent<Rigidbody>();
		var collider = obj.AddComponent<MeshCollider>();
		var hover = obj.AddComponent<HoverShader>();
		var pickup = obj.AddComponent<PickUpItem>();

		collider.convex = true;
		hover.hoverShader = hoverShader;
		pickup.item = item;

		return obj;
	}
}
