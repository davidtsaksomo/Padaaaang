using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMakanan : MonoBehaviour {

	GameObject[] foodPos;
	GameObject[] foods; 

	public GameObject foodPrefab;
	public Transform parent;
	// Use this for initialization
	void Start () {
		foodPos = GameObject.FindGameObjectsWithTag ("FoodHolder");
		Debug.Log (foodPos.Length);
		foods = new GameObject[foodPos.Length]; 
		for(int i = 0; i < foodPos.Length; i++){
			foods[i] = (GameObject)Instantiate (foodPrefab, foodPos[i].transform.localPosition,Quaternion.identity,parent);
			foods [i].transform.localPosition = foodPos [i].transform.localPosition;
			//penbghitungan foodID
			int foodID = i%FoodDatabase.count;
			foods[i].GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [foodID].image;
			foods[i].GetComponent<FoodID> ().id = foodID;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
