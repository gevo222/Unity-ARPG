using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDeactivates : MonoBehaviour {

	[SerializeField]
	public Transform watchPosition;

	private Vector3? snapshot;

	void OnEnable(){
        StartCoroutine(SnapshotPosition());
	}

	private IEnumerator SnapshotPosition(){
        yield return new WaitForSeconds(1.5f);
		snapshot = watchPosition.position;
	}

	void OnDisable(){
		snapshot = null;
	}

	void Update(){
		if(snapshot != null && snapshot != watchPosition.position){
			transform.gameObject.SetActive(false);
		}
	}
}
