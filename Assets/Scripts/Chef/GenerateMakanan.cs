using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMakanan : MonoBehaviour {

	public GameObject[] foodPos;
	public GameObject[] foods; 

	public GameObject foodPrefab;
	public Transform parent;
	public GameObject statusBarPrefab;
	// Use this for initialization
	void Start () {
		int countchef = CountChef ();
		if (countchef <= 0) {
			countchef = 1;
		}
		Debug.Log (foodPos.Length);

		int myfood;
		if (countchef > 1) {
			myfood = (foodPos.Length / 3) * 2;
		} else {
			myfood = foodPos.Length;
		}
		foods = new GameObject[myfood]; 

		bool terbesar = true;
		bool terkecil = true;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.red) {
				if (player.ID > PhotonNetwork.player.ID) {
					terbesar = false;
				} else if (player.ID > PhotonNetwork.player.ID){
					terkecil = false;
				}
			}
		}
		int start, end;
		if (terbesar) {
			start = 0;
			end = start + myfood;
		} else if (terkecil) {
			end = foodPos.Length;
			start = end - myfood;
		} else {
			start = (foodPos.Length - myfood) / 2;
			end = start + myfood;
		}
		int neff = 0;
		for(int i = 0; i < foodPos.Length; i++){
			if (i >= start && i < end) {

				foods [neff] = (GameObject)Instantiate (foodPrefab, foodPos [i].transform.localPosition, Quaternion.identity, parent);
				foods [neff].transform.localPosition = foodPos [i].transform.localPosition;
				//pennghitungan foodID
				int foodID = i % FoodDatabase.count;
				if (foodID == 0) {
					foodID = Random.Range (1, FoodDatabase.count);
				}
				foods [neff].GetComponent<Image> ().sprite = FoodDatabase.instance.foodDatabase [foodID].image;
				foods [neff].GetComponent<FoodID> ().id = foodID;
				//assigning status bar
				if (statusBarPrefab != null) {
					Vector2 position = new Vector2 (
						                   foodPos [i].transform.localPosition.x,
						                   foodPos [i].transform.localPosition.y - 180);
					GameObject statusBar = Instantiate (statusBarPrefab, position, Quaternion.identity, foodPos [i].transform);
					FoodStatusBar statusBarScript = statusBar.GetComponent<FoodStatusBar> ();
					statusBarScript.food = foods [neff];
					statusBarScript.enabled = true;
				}
				neff++;
			} else {
				foodPos [i].SetActive (false);
			}


		}
		
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


}
