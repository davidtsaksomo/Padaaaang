using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMakanan : MonoBehaviour {

	GameObject[] foodPos;
	GameObject[] foods; 

	public GameObject foodPrefab;
	public Transform parent;
	public GameObject statusBarPrefab;
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
			if (foodID == 0) {
				foodID = Random.Range(1,FoodDatabase.count);
			}
			foods[i].GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [foodID].image;
			foods[i].GetComponent<FoodID> ().id = foodID;
			//assigning status bar
			if (statusBarPrefab != null) {
				Vector2 position = new Vector2(
					foodPos[i].transform.localPosition.x,
					foodPos[i].transform.localPosition.y -180);
				GameObject statusBar = Instantiate (statusBarPrefab, position, Quaternion.identity, foodPos [i].transform);
				FoodStatusBar statusBarScript = statusBar.GetComponent<FoodStatusBar> ();
				statusBarScript.food = foods [i];
				statusBarScript.enabled = true;
			}

		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
