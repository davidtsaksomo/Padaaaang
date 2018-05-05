using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPrefab : MonoBehaviour {

	public GameObject customer;
	public List<GameObject> foodOrder = new List<GameObject>();
	public int AngryTime;
	public int indexPos = -1;

	void Start(){
		StartCoroutine ("CountDown", AngryTime);
	}

	IEnumerator CountDown(int time){
		while (time > 0) {
			yield return new WaitForSeconds (1f);
			time--;
			if(time <=4){
				customer.GetComponent<Image> ().color = Color.red;
			}
		}
		GenerateOrder.instance.Failure ();
		Debug.Log(indexPos+" marah");
		if (indexPos >= 0) {
			GenerateOrder.orders [indexPos] = null;
		}
		else {
			GenerateOrder.pendingorders--;
			GenerateOrder.instance.UpdateQueueText();
		}
		GenerateOrder.instance.UpdateOrderPosition ();
		GenerateOrder.OrderList.Remove (gameObject);
		GenerateOrder.instance.CheckDone ();
		Destroy (gameObject);

	}
}
