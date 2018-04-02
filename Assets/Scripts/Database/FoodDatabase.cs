using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDatabase : MonoBehaviour {
	//singleton
	public static FoodDatabase instance = null;
	public static int count;
	[SerializeField] public Food[] foodDatabase;
	// Use this for initialization
	void Awake(){
		if (instance == null) {
			instance = this;
			count = foodDatabase.Length;
		} else {
			Destroy (this);
		}
	}
	void Start(){
	}
}

[System.Serializable]
public class Food {
	public string name;
	public Sprite image;
}