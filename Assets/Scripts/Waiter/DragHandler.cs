using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	public GameObject[] arrayOrder ;

	Vector3 startPosition;

	void Start() {
		
	}

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
		bool found = false;
		int i = 0;
		/*
		while (i < arrayOrder.Length && !found) {
			GameObject destination = GameObject.Find(arrayOrder[i].gameObject.name);
			if (WithinRange (destination)) {
				found = true;
				if (destination.tag == gameObject.tag) {
					Destroy (gameObject);
					Destroy (destination);
				} else if (destination.tag == "trash") {
					Destroy (gameObject);
				} else {
					Debug.Log ("Something wrong");
				}
			}
			i++;
		}*/


		GameObject destination = arrayOrder [GenerateOrder.orderPos [i]].gameObject;
		if (WithinRange () > 0) {
			if (WithinRange () == 1) { // left
				Destroy (GameObject.Find(arrayOrder [GenerateOrder.orderPos [0]].gameObject.name));
				Debug.Log (GameObject.Find(arrayOrder [GenerateOrder.orderPos [0]].gameObject.name));
				GenerateOrder.orderPos [0] = 999;
			} else if (WithinRange () == 2) { //middle
				Destroy (GameObject.Find(arrayOrder [GenerateOrder.orderPos [1]].gameObject.name));
				GenerateOrder.orderPos [1] = 999;
			} else if (WithinRange () == 3) {
				Destroy (GameObject.Find(arrayOrder [GenerateOrder.orderPos [2]].gameObject.name));
				GenerateOrder.orderPos [2] = 999;
			} 
			Destroy (this.gameObject);
		} else {
			itemBeingDragged = null;
			transform.position = startPosition;
		}

	}

	#endregion

	int WithinRange(){
		RectTransform rectTransform = this.gameObject.GetComponent<RectTransform> ();
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
		


	void Update() {

	}
		
}
