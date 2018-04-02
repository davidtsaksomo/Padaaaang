using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChefNetworkManager : MonoBehaviour {

	PhotonView photonView;
	WindowManager windowmanager;
	// Use this for initialization
	void Awake () {
		photonView = GetComponent<PhotonView> ();
		windowmanager = GetComponent<WindowManager> ();
		windowmanager.windowSum = CountWaiter ();
	}
	
	// Update is called once per frame
	public void SendFood (int foodID, int waiterID) {
		photonView.RPC ("InstantiateFood",PhotonTargets.Others, foodID, waiterID);
	}

	int CountWaiter(){
		int blue = 0;
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.blue) {
				windowmanager.window [blue].GetComponent<Window> ().id = player.ID;
				windowmanager.window [blue].GetComponent<Window> ().nama.text = player.NickName;
				blue++;
			}
		}
		return blue;
	}

	[PunRPC]
	void GameOver(){
		PhotonNetwork.LoadLevel ("GameOver");
	}
}
