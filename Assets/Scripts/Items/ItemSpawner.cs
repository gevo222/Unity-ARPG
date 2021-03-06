using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class ItemSpawner : MonoBehaviour {

	public static ItemSpawner main;

	[SerializeField]
	public Transform playerPosition;
	[SerializeField]
	public GameObject parent;
	[SerializeField]
	public Shader hoverShader;
	[SerializeField]
	public float itemScaleFactor = 1.2f;


	void Start(){
		ItemSpawner.main = this;
	}

	public GameObject Spawn(ItemObject item, Vector3 position){
		var obj = Object.Instantiate(item.WorldPrefab, position, Quaternion.identity);

		obj.transform.SetParent(parent.transform, false);
		obj.transform.localScale *= itemScaleFactor;
		obj.transform.localEulerAngles += new Vector3(
			Random.Range(0,360), Random.Range(0,360), Random.Range(0,360)
		);

		var rigidbody = obj.AddComponent<Rigidbody>();
		var collider = obj.AddComponent<BoxCollider>();
		var hover = obj.AddComponent<HoverShader>();
		var pickup = obj.AddComponent<PickUpItem>();

		//collider.convex = true;
		hover.hoverShader = hoverShader;
		rigidbody.mass = 25;
		pickup.item = item;

		return obj;
	}

	public GameObject SpawnNear(ItemObject item, Vector3 position, float factor = 1.0f){
		var nearby = new Vector3(
			Random.Range(-1.5f, 1.5f) * factor,
			Random.Range(1, 3),
			Random.Range(-1.5f, 1.5f) * factor
		);
		return Spawn(item, position + nearby);
	}

	public GameObject SpawnNearPlayer(ItemObject item){
		return SpawnNear(item, playerPosition.position);
	}
}
