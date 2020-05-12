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
		//SpawnNearPlayer(ItemObject.All["Items/Amulets/PauaAmulet"]);
		//SpawnNearPlayer(ItemObject.All["Items/Boots/EelskinBoots"]);
		//SpawnNearPlayer(ItemObject.All["Items/Weapons/Cutter"]);
		//SpawnNearPlayer(ItemObject.All["Items/Weapons/Epic_Dagger"]);
	}

	public GameObject Spawn(ItemObject item, Vector3 position){
		var obj = Object.Instantiate(item.WorldPrefab, position, Quaternion.identity);

		obj.transform.SetParent(parent.transform, false);
		obj.transform.localScale *= itemScaleFactor;
		obj.transform.localEulerAngles += new Vector3(
			0, Random.Range(0,360), Random.Range(60, 120)
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

	public GameObject SpawnNearPlayer(ItemObject item){
		var nearby = new Vector3(
			Random.Range(-1.5f, 1.5f), Random.Range(2, 3), Random.Range(-1.5f, 1.5f)
		);
		return Spawn(item, playerPosition.position + nearby);
	}
}
