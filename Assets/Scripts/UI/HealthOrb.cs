using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthOrb : MonoBehaviour {

	private int currentHealth;

	public void OnUpdateHealth(Transform parent, int hp){
		currentHealth = hp;

	}
}
