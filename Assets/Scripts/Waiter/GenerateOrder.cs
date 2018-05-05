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

	public static int pendingorders = 0;
	public static GenerateOrder instance;
	public static List<GameObject> foodActive = new List<GameObject> ();
	public static GameObject[] orders = new GameObject[3]; // 0-2
	public static GameObject[] foods = new GameObject[3]; // 0-2
	public GameObject orderPrefab;
	public GameObject orderPrefab2;
	public GameObject foodPrefab;
	public Transform canvas;
	public Text queueCount;
	public static List<GameObject> OrderList = new List<GameObject>();

	public float startTime = 3f;
	public float orderTime = 3f;
	public int orderNumber = 10;
	public float timeDiffPerLevel = 0.2f;
	public int numberDiffPerLevel = 5;

	int orderinstantianted = 0;

	//Network
	PhotonView photonView;

	//Singleton Principle
	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
		OrderList.Clear ();
		foodActive.Clear ();
		pendingorders = 0;

	}
	// Use this for initialization
	void Start () {
		//difficulty increment
		orderTime = orderTime - ((LevelController.level - 1) * timeDiffPerLevel);
		if (orderTime < 2.5f) {
			orderTime = 2.5f;
		}
		orderNumber = orderNumber + ((LevelController.level - 1) * numberDiffPerLevel);

		int countwaiter = CountWaiter ();
		int countchef = CountChef ();

		//balancing
		orderTime = orderTime + ((countwaiter-countchef) * orderTime / 2) + ((countwaiter + countchef - 2) * orderTime / 4);
		orderNumber = orderNumber - ((countwaiter - 1) * orderNumber / 3);

		//Photon inizialitation
		photonView = GetComponent<PhotonView> ();

		InvokeRepeating ("InstantiateOrder", startTime, orderTime);	
		//inisialisasi posisi order
		for(int i=0; i<=2; i++) {
			orders[i] = null;
			foods [i] = null;
		}
	}

	public void UpdateOrderPosition() {
		GameObject order;
		if (orders [0] == null && orders[1] != null) { //kiri kosong & tengah isi
			SetOrderLeft (orders[1]);
			orders [0] = orders [1];
			orders [1] = null;
		}

		if (orders[1] == null && orders[2] != null) { //tengah kosong & kanan isi
			SetOrderMiddle (orders[2]);
			orders [1] = orders [2];
			orders [2] = null;
		}
		if(pendingorders > 0){
			UpdatePendingOrders ();
		}
	}
	void UpdatePendingOrders() {
		foreach (GameObject order in OrderList) {
			if (order.GetComponent<OrderPrefab> ().indexPos == -1) {
				if (orders [0] == null) { //kiri kosong
					SetOrderLeft (order);
					orders [0] = order;
					pendingorders--;
				} else if (orders [1] == null) { //kiri isi, tengah kosong
					SetOrderMiddle (order);
					orders [1] = order;
					pendingorders--;
				} else if (orders [2] == null) { //kiri isi, tengah isi, kanan kosong
					SetOrderRight (order);
					orders [2] = order;
					pendingorders--;
				}
				return;
			}
		}
	}
	void SetOrderLeft(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left1, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right1, -top);
		order.GetComponent<OrderPrefab> ().indexPos = 0;
	}

	void SetOrderMiddle(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left2, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right2, -top);
		order.GetComponent<OrderPrefab> ().indexPos = 1;

	}

	void SetOrderRight(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left3, bottom);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right3, -top);
		order.GetComponent<OrderPrefab> ().indexPos = 2;

	}
	void SetOrderPending(GameObject order) {
		order.GetComponent<RectTransform> ().offsetMin = new Vector2 (left3, 3000f);
		order.GetComponent<RectTransform> ().offsetMax = new Vector2 (-right3, (-top-bottom) + 3000f);
		order.GetComponent<OrderPrefab> ().indexPos = -1;

	}
	bool isMember(int[] foodId, int id) {
		for (int i = 0; i < foodId.Length; i++) {
			if (foodId [i] == id) {
				return true;
			}
		}
		return false;
	}
		
	void InstantiateOrder() {
		if(orderinstantianted < orderNumber){
			//order penuh

			int foodOrderSize = Random.Range (1, 3);
			GameObject newOrder = null;
			int customerID = Random.Range(0, CustomerDatabase.count);

			if (foodOrderSize == 1) {
				newOrder = (GameObject)Instantiate (orderPrefab, canvas);
			} else if (foodOrderSize == 2) {
				newOrder = (GameObject)Instantiate (orderPrefab2, canvas);
			}

			int[] foodID = new int[foodOrderSize];
			foodID[0] = Random.Range(0, FoodDatabase.count);
		
			for (int i = 1; i < foodOrderSize; i++) { 
				int id = Random.Range (0, FoodDatabase.count);
				while (isMember (foodID, id)) {
					id = Random.Range (0, FoodDatabase.count);
				}
				foodID [i] = id;
			}	

			OrderPrefab newOrderRef = newOrder.GetComponent<OrderPrefab> ();
			newOrderRef.customer.GetComponent<Image> ().sprite = CustomerDatabase.instance.customerDatabase [customerID].image;
			newOrderRef.AngryTime = CustomerDatabase.instance.customerDatabase [customerID].angryTime;
			for (int i = 0; i < foodOrderSize; i++) {
				newOrderRef.foodOrder [i].GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [foodID [i]].image;
				newOrderRef.foodOrder [i].GetComponent<FoodID> ().id = foodID[i];
			}

			if (newOrder != null) {
				OrderList.Add (newOrder);
				if (orders [0] == null) { //kiri kosong
					SetOrderLeft (newOrder);
					orders [0] = newOrder;
				} else if (orders [1] == null) { //kiri isi, tengah kosong
					SetOrderMiddle (newOrder);
					orders [1] = newOrder;
				} else if (orders [2] == null) { //kiri isi, tengah isi, kanan kosong
					SetOrderRight (newOrder);
					orders [2] = newOrder;
				} else {
					SetOrderPending (newOrder);
					pendingorders++;
				}
			}
			UpdateQueueText ();
			orderinstantianted++;
		}
	}

	[PunRPC]
	void InstantiateFood(int foodID, int waiterID) {
		if (PhotonNetwork.player.ID == waiterID) {
			if (foodActive.Count < 3) {
				GameObject newFood = (GameObject)Instantiate (foodPrefab, canvas);
				newFood.GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [foodID].image;
				newFood.GetComponent<FoodID> ().id = foodID;

				if (foodActive.Count == 0) { //no food, set position center
					newFood.GetComponent<RectTransform> ().localPosition = new Vector2 (12f, -1445f);
					foodActive.Add (newFood);
				} else if (foodActive.Count == 1) {
					foodActive [0].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (-418f, -1445f);
					newFood.GetComponent<RectTransform> ().localPosition = new Vector2 (403f, -1445f);
					foodActive.Add (newFood);
				} else if (foodActive.Count == 2) {
					foodActive [0].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (-627f, -1445f);
					foodActive [1].gameObject.GetComponent<RectTransform> ().localPosition = new Vector2 (12f, -1445f);
					newFood.GetComponent<RectTransform> ().localPosition = new Vector2 (635f, -1445f);
					foodActive.Add (newFood);
				}
			} else {
				Instantiate (Resources.Load("KebuangText"), canvas);
			}

		}
	}


	public void UpdateQueueText(){
		if (pendingorders == 0) {
			queueCount.text = "";
		} else {
			queueCount.text = "+" + pendingorders;
		}
	}

	int CountWaiter(){
		int blue = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.blue) {
				blue++;
			}
		}
		return blue;
	}
	int CountChef(){
		int red = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.red) {
				red++;
			}
		}
		return red;
	}
	public void CheckDone(){
		if (pendingorders == 0 && orders [0] == null && orders [1] == null && orders [2] == null && orderinstantianted >= orderNumber) {
			//Pun RPC in script Game Controller
			photonView.RPC ("Done", PhotonTargets.MasterClient);
		}
	}
	public void Failure(){
		//Pun RPC in script Game Controller
		photonView.RPC ("Failure", PhotonTargets.MasterClient);
	}
}
