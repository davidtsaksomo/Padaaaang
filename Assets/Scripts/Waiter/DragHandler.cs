using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	GameObject[] dragDestinations;
	Vector3 startPosition;
	public GameObject foodPrefab;
	GameObject canvasObject;
	Transform canvas;

	void Start(){
		dragDestinations = GameObject.FindGameObjectsWithTag ("DragDestination");
		canvasObject = GameObject.FindGameObjectWithTag("waiterCanvas");
		canvas = canvasObject.GetComponent<Transform>();
	}

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("begin drag");
		if (gameObject.tag == "rice") {
			itemBeingDragged = (GameObject)Instantiate (foodPrefab, canvas);
			itemBeingDragged.GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [0].image;
			itemBeingDragged.GetComponent<FoodID> ().id = 0;
			itemBeingDragged.GetComponent<Transform> ().position = transform.position;
		} else {
			itemBeingDragged = gameObject;
		}
		startPosition = transform.position;
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		itemBeingDragged.GetComponent<Transform>().position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		GameObject order;
		int withinRange = WithinRange ();
		if (withinRange > 0) {
			order = GenerateOrder.orders [withinRange - 1];
			int idxMatchOrder = MatchOrder (itemBeingDragged, order);
			if (idxMatchOrder != -1) {
				OrderPrefab newOrderRef = order.GetComponent<OrderPrefab> ();
				if (newOrderRef.foodOrder.Count > 1) {
					order.transform.GetChild (2 + idxMatchOrder).gameObject.SetActive (false);
					order.transform.GetChild (order.transform.childCount - 1).gameObject.SetActive (false);
					newOrderRef.foodOrder.RemoveAt (idxMatchOrder);
				} else {
					Destroy (order);
					Destroy (itemBeingDragged);
					GenerateOrder.orders [withinRange - 1] = null;
					GenerateOrder.instance.UpdateOrderPosition ();
					GenerateOrder.foodActive.Remove (itemBeingDragged);
				}
			}
		} else {
			bool found = false;
			int i = 0;
			while (i < dragDestinations.Length && !found) {
				if (WithinRange (dragDestinations [i])) {
					found = true;
					switch (dragDestinations [i].name.Substring (0, 3)) {
					case "Tra":
						GenerateOrder.foodActive.Remove (itemBeingDragged);
						Destroy (itemBeingDragged);
						break;
					}
				}
				i++;

			}
		}

		if (this.tag == "rice") {
			Destroy (itemBeingDragged);
		} 
		itemBeingDragged.GetComponent<Transform> ().position = startPosition;
		itemBeingDragged = null;
	}

	#endregion

	int WithinRange(){
		RectTransform rectTransform = itemBeingDragged.GetComponent<RectTransform> ();
		if (rectTransform.localPosition.y >= -107 && rectTransform.localPosition.y <= 1000) {
			if (rectTransform.localPosition.x >= -800 && rectTransform.localPosition.x <= -450) {
				return 1; //left
			} else if (rectTransform.localPosition.x >= -180 && rectTransform.localPosition.x <= 150) {
				return 2; // middle
			} else if (rectTransform.localPosition.x >= 475 && rectTransform.localPosition.x <= 755) { 
				return 3; //right
			} else {
				return 0; //not in range
			}
		} else {
			//cek di trash atau bukan
			return 0;
		}
	}

	bool WithinRange(GameObject gameObject){
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		if (gameObject.activeInHierarchy == false) {
			return false;
		}
		bool withinXRange = Mathf.Abs(GetComponent<RectTransform>().localPosition.x - rectTransform.localPosition.x) < (rectTransform.sizeDelta.x / 2);
		bool withinYRange = Mathf.Abs(GetComponent<RectTransform>().localPosition.y - rectTransform.localPosition.y) < (rectTransform.sizeDelta.y / 2);

		return withinXRange && withinYRange;
	}
		
	int MatchOrder(GameObject food, GameObject order) {
		OrderPrefab newOrderRef = order.GetComponent<OrderPrefab> ();
		for (int i = 0; i < newOrderRef.foodOrder.Count; i++) {
			if (food.GetComponent<FoodID> ().id == newOrderRef.foodOrder [i].GetComponent<FoodID> ().id) {
				return i;
			}
		}
		return -1;
	}
}
