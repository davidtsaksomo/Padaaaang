using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telepon : MonoBehaviour {

	[SerializeField] Slider progBar;
	[SerializeField] Text nama;
	[SerializeField] Image telepon;
	[SerializeField] GameObject papannama;


	bool isHold = false;
	float value;
	float index;
	bool inactive = false;

	public float holdTime = 1.5f;
	public float coolDownTime = 5f;

	int currentID = -1;
	int currentIndex = 0;
	PhotonView photonView;
	List<PhotonPlayer> players = new List<PhotonPlayer>();

	bool notUsed;
	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView> ();
		progBar.value = 0f;
		progBar.interactable = false;

		players.Clear ();
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			if (player.GetTeam () == PunTeams.Team.red) {
				players.Add (player);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isHold) {
			value += 1f/holdTime* Time.deltaTime;
			progBar.value = value;
		} else {
			value -= 1f* Time.deltaTime;
			progBar.value = value;
		}
		if (value > 1f) {
			value = 0f;
			ChangeName ();
		}
		if (value < 0f) {
			value = 0f;
		}
	}
	public void PointerEvent(string msg){
		if (inactive)
			return;
		
		if (msg == "Down") {
			isHold = true;
			papannama.SetActive (true);
		} else if (msg == "Exit") {
			isHold = false;
			Clear ();
			papannama.SetActive (false);

		} else if (msg == "Up" && isHold) {
			isHold = false;
			papannama.SetActive (false);
			Call ();
		}
	}

	void Call(){
		if (currentID >= 0) {

			photonView.RPC ("PanggilSatpam", PhotonTargets.All,currentID);
			telepon.color = Color.gray;
			inactive = true;
			Invoke ("active", coolDownTime);
		}
		Clear ();
	}

	void active(){
		inactive = false;
		telepon.color = Color.white;
	}
	void ChangeName(){
		if (players.Count > 0) {
			nama.text = players [currentIndex].NickName;
			currentID = players [currentIndex].ID;
			currentIndex++;
			if (currentIndex >= players.Count) {
				currentIndex = 0;
			}
		} else {
			nama.text = "";
		}

	}
	void Clear(){
		currentID = -1;
		nama.text = ""; 
	}
}
