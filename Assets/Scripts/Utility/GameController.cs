using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {
	PhotonView photonview;
	int donetarget;
	[SerializeField] int maxfailure;
	int done = 0;
	int failure = 0;
	void Awake(){
		photonview = GetComponent<PhotonView> ();
	}
	// Use this for initialization
	void Start () {			
		donetarget = CountWaiter ();
		maxfailure = maxfailure * donetarget;

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
	[PunRPC]
	void Done(){
		if (PhotonNetwork.isMasterClient) {
			done++;
			if (done >= donetarget && failure < maxfailure) {
				//Level Complete Here
				photonview.RPC ("GameComplete", PhotonTargets.All);

			}
		}

	}
	[PunRPC]
	void Failure(){
		if (PhotonNetwork.isMasterClient) {
			failure++;
			if (failure >= maxfailure) {
				//Level End Here

				photonview.RPC ("GameOver", PhotonTargets.All);
			} else {
				float percentage = 1f - (failure / (float) maxfailure);
				photonview.RPC ("UpdateBar", PhotonTargets.All, percentage);

			}
		}
	}
	[PunRPC]
	void GameOver(){
		PhotonNetwork.LoadLevel ("GameOver");
	}
	[PunRPC]
	void GameComplete(){
		LevelController.level++;
		PhotonNetwork.LoadLevel ("LevelStart");
	}
}