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
	public static int[] orderPos = new int[3]; // 0-2

	public GameObject[] Orders;
	public Transform canvas;

	// Use this for initialization
	void Start () {
		nextCustTime = Time.time + 1.0f;	
		//inisialisasi posisi order
		for(int i=0; i<=2; i++) {
			orderPos[i] = 999;
		}
	}
	
	// Update is called once per frame
	void Update () {
		GameObject order, newOrder;
		Debug.Log ("order[0] : "+ orderPos [0]);
		Debug.Log ("order[1] : "+ orderPos [1]);
		Debug.Log ("order[2] : "+ orderPos [2]);
		//update order position
		if (orderPos [0] == 999 && orderPos[1] != 999) { //kiri kosong & tengah isi
			order = GameObject.Find(Orders[orderPos[1]].gameObject.name);
			Debug.Log ("name: "+GameObject.Find(Orders[orderPos[1]].gameObject.name).gameObject.name);
			SetOrderLeft (order);
			orderPos [0] = orderPos [1];
			orderPos [1] = 999;
		}

		if (orderPos[1] == 999 && orderPos[2] != 999) { //tengah kosong & kanan isi
			order = GameObject.Find(Orders[orderPos[2]].gameObject.name);
			SetOrderMiddle (order);
			orderPos [1] = orderPos [2];
			orderPos [2] = 999;
		}

		//instantiate order
		if(Time.time > nextCustTime)
		{
			int orderNum = Random.Range(0,5);
			Debug.Log (orderNum);

			if (orderPos [0] == 999) { //kiri kosong
				newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
				SetOrderLeft (newOrder);
				orderPos [0] = orderNum;
			} else if (orderPos [1] == 999) { //kiri isi, tengah kosong
				newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
				SetOrderMiddle (newOrder);
				orderPos [1] = orderNum;
			} else if (orderPos [2] == 999) { //kiri isi, tengah isi, kanan kosong
				newOrder = (GameObject)Instantiate (Orders [orderNum], canvas);
				SetOrderRight (newOrder);
				orderPos [2] = orderNum;
			}

			//increment next_spawn_time
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

	public GameObject[] GetOrders() {
		return Orders;
	}
}
