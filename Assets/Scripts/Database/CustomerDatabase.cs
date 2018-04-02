using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDatabase : MonoBehaviour {
	//singleton
	public static CustomerDatabase instance = null;
	public static int count;
	[SerializeField] public Customer[] customerDatabase;
	// Use this for initialization
	void Awake(){
		
		if (instance == null) {
			instance = this;
			count = customerDatabase.Length;
		} else {
			Destroy (this);
		}
	}
}

[System.Serializable]
public class Customer {
	public string name;
	public Sprite image;
}