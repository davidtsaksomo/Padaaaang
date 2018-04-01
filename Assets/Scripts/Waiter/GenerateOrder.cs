using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateOrder : MonoBehaviour {
	float left1 = -462.7f;
	float left2 = 153.3f;
	float left3 = 763.3f;
	float right1 = 783.3f;
	float right2 = 167.3f;
	float right3 = -442.7f;
	float top = -884.5f;
	float bottom = 285.5f;

	float nextCustTime;
	public static List<GameObject> foodActive = new List<GameObject> ();
	public static int[] orderPos = new int[3]; // 0-2
	public static int[] foodPos = new int[3]; // 0-2

	public GameObject[] Orders;
	public GameObject[] Foods;
	public Transform canvas;

	// Use this for initialization
	void Start () {
		nextCustTime = Time.time + 1.0f;	
		//inisialisasi posisi order
		for(int i=0; i<=2; i++) {
			orderPos[i] = 999;
			foodPos [i] = 999;
		}
	}
	
	// Update is called once per frame
	void Update () {
		GameObject newOrder;
		UpdateOrderPosition ();
		UpdateFoodPosition ();
		//InstantiateFood (get object tag from chef); 

		if(Time.time > nextCustTime)
		{
			InstantiateOrder ();
			InstantiateFood (); //TODO: get object tag from chef

			nextCustTime += 5.0f;
		}
	}

	void SetOrderLeft(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left1, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right1, -top);
	}

	void SetOrderMiddle(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left2, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right2, -top);
	}

	void SetOrderRight(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left3, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right3, -top);
	}

	GameObject GetOrder(int i) {
		string orderName = Orders [GenerateOrder.orderPos [i]].gameObject.name + "(Clone)";
		GameObject order = GameObject.Find (orderName);
		return order;
	}

	void UpdateOrderPosition() {
		GameObject order;
		if (orderPos [0] == 999 && orderPos[1] != 999) { //kiri kosong & tengah isi
			order = GetOrder(1);
			SetOrderLeft (order);
			orderPos [0] = orderPos [1];
			orderPos [1] = 999;
		}

		if (orderPos[1] == 999 && orderPos[2] != 999) { //tengah kosong & kanan isi
			order = GetOrder(2);
			SetOrderMiddle (order);
			orderPos [1] = orderPos [2];
			orderPos [2] = 999;
		}
	}

	void UpdateFoodPosition() {
		GameObject order;
		if (foodActive.Count == 1) { 
			//foodActive [0].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (12f, -1445f);
			//Debug.Log (foodActive [0].gameObject.name);
		} else if (foodActive.Count == 2) { 
			//foodActive [0].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (-418f, -1445f);
			//Debug.Log (foodActive [0].gameObject.name);
			//foodActive [1].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (403f, -1445f);
			//Debug.Log (foodActive [1].gameObject.name);
		} else if (foodActive.Count == 3) { 
			//foodActive[0].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-627f, -1445f);
			//foodActive[1].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(12f, -1445f);
			//foodActive[2].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2(635f, -1445f);
		}
	}

	void InstantiateOrder() {
		GameObject newOrder;
		int orderNum = Random.Range(0,5);

		if (orderPos [0] == 999) { //kiri kosong
			newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
			Debug.Log (orderNum);
			orderPos [0] = orderNum;
			Debug.Log (orderPos[0]);
		} else if (orderPos [1] == 999) { //kiri isi, tengah kosong
			newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
			SetOrderMiddle (newOrder);
			orderPos [1] = orderNum;
		} else if (orderPos [2] == 999) { //kiri isi, tengah isi, kanan kosong
			newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
			SetOrderRight (newOrder);
			orderPos [2] = orderNum;
		}
	}

	void InstantiateFood() {
		GameObject newFood;
		int foodNum = Random.Range(0,5);

		if (foodActive.Count == 0) { //no food, set position center
			newFood = (GameObject)Instantiate (Foods[foodNum], canvas);
			newFood.GetComponent<RectTransform> ().localPosition = new Vector2(12f, -1445f);
			foodActive.Add (newFood);
		} else if (foodActive.Count == 1) {
			newFood = (GameObject)Instantiate (Foods[foodNum], canvas);
			foodActive[0].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-418f, -1445f);
			newFood.GetComponent<RectTransform> ().localPosition = new Vector2(403f, -1445f);
			foodActive.Add (newFood);
		} else if (foodActive.Count == 2) {
			newFood = (GameObject)Instantiate (Foods[foodNum], canvas);
			foodActive[0].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-627f, -1445f);
			foodActive[1].gameObject.GetComponent<RectTransform>().localPosition = new Vector2(12f, -1445f);
			newFood.GetComponent<RectTransform> ().localPosition = new Vector2(635f, -1445f);
			foodActive.Add (newFood);
		}
	}
}
