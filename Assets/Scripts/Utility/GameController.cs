using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {
	Room room;
	[SerializeField] GameObject chefInterface;
	[SerializeField] GameObject waiterInterface;
	PhotonView photonView;
	[SerializeField] Text menus;
	bool isChef = false;
	bool isWaiter = false;
	void Awake(){
		photonView = GetComponent<PhotonView> ();
		room = PhotonNetwork.room;
	}
	// Use this for initialization
	void Start () {			
		menus.text = "";
		if (PhotonNetwork.player.GetTeam() == PunTeams.Team.red) {
			chefInterface.SetActive (true);
			isChef = true;
		} else if(PhotonNetwork.player.GetTeam() == PunTeams.Team.blue){
			waiterInterface.SetActive (true);
			isWaiter = true;
		}
	}
	
	// Update is called once per frame
	public void SendMenu (string name) {
		photonView.RPC ("GetMenu", PhotonTargets.All, name);
	}
	[PunRPC]
	void GetMenu (string name){
		if(isWaiter){
			menus.text += "\n"+name;
		}
	}

	public void GameOver() {
		Application.LoadLevel ("GameOver");
	}
}
