using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	public GameObject[] arrayOrder ;

	Vector3 startPosition;

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		GameObject order;

		if (WithinRange () > 0) {
			if (WithinRange () == 1) { // left
				order = GetOrder(0);
				if (MatchOrder(gameObject, order)) {
					Destroy (order);
					Destroy (gameObject);
					GenerateOrder.orderPos [0] = 999;
					Debug.Log ("count before remove "+GenerateOrder.foodActive.Count);
					GenerateOrder.foodActive.Remove (gameObject);
					Debug.Log ("count after remove "+GenerateOrder.foodActive.Count);
				}
			} else if (WithinRange () == 2) { //middle
				order = GetOrder(1);
				if (MatchOrder (gameObject, order)) {
					Destroy (order);
					Destroy (gameObject);
					GenerateOrder.orderPos [1] = 999;
					Debug.Log ("count before remove "+GenerateOrder.foodActive.Count);
					GenerateOrder.foodActive.Remove (gameObject);
					Debug.Log ("count after remove "+GenerateOrder.foodActive.Count);
				}
			} else if (WithinRange () == 3) { //right
				order = GetOrder(2);
				if (MatchOrder (gameObject, order)) {
					Destroy (order);
					Destroy (gameObject);
					GenerateOrder.orderPos [2] = 999;
					Debug.Log ("count before remove "+GenerateOrder.foodActive.Count);
					GenerateOrder.foodActive.Remove (gameObject);
					Debug.Log ("count after remove "+GenerateOrder.foodActive.Count);
				}
			} 
		} 
		itemBeingDragged = null;
		transform.position = startPosition;
	}

	#endregion

	int WithinRange(){
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
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

	GameObject GetOrder(int i) {
		string orderName = arrayOrder [GenerateOrder.orderPos [i]].gameObject.name + "(Clone)";
		GameObject order = GameObject.Find (orderName);
		return order;
	}
		
	bool MatchOrder(GameObject food, GameObject order) {
		if (food.tag == order.tag) {
			return true;
		} else {
			return false;
		}
	}
}
