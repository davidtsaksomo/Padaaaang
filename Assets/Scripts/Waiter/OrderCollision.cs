using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (gameObject.tag == collision.gameObject.tag) {
			Destroy (gameObject);
		}
	}
}
