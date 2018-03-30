using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodPickUp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
	public static GameObject itemBeingDragged;
	public GameObject[] dragDestinations;
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
		ActivateDrag ();

		itemBeingDragged = null;
		transform.position = startPosition;
	}

	#endregion

	void ActivateDrag(){
		bool found = false;
		int i = 0;
		while (i < dragDestinations.Length && !found) {
			if (WithinRange (dragDestinations [i])) {
				found = true;
				Debug.Log (dragDestinations[i].name.Substring(0,1));
			}
			i++;
		}
	}

	bool WithinRange(GameObject gameObject){
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		bool withinXRange = Mathf.Abs(transform.localPosition.x - rectTransform.localPosition.x) < (rectTransform.sizeDelta.x / 2);
		bool withinYRange = Mathf.Abs(transform.localPosition.y - rectTransform.localPosition.y) < (rectTransform.sizeDelta.y / 2);
		return withinXRange && withinYRange;
	}
}
