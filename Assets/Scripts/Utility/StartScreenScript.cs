using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenScript : MonoBehaviour {

	public GameObject background;
	public GameObject backgroundBlur;
	public GameObject inputName;
	public GameObject inputRoomName;
	public InputField nameField;
	public InputField roomNameField;
	public float stopTimer;
	private float curTimer;
	/**
	 * Status:
	 * home screen: 0
	 * input name transition down: 11
	 * input name stop: 12
	 * input name transition up, go to home: 131
	 * input name transition up, go to input room name: 132
	 * input name transition down: 21
	 * input name stop: 22
	 * input name transition up: 23
	 **/
	private int status;

	//Reference to info text
	[SerializeField] Text infoText;
	[SerializeField] Text statusText;
	//Properties for room
	private string roomName;
	private RoomInfo[] roomsList;
	private RoomOptions roomopt;

	bool inLobby = false;

	// Use this for initialization
	void Start () {
		displayHome ();
		PlayerPrefs.DeleteKey ("roomname");
		nameField.GetComponent<InputField>().text = PlayerPrefs.GetString ("playerName", "Player");
		status = 0;
		MusicManager musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		if (!musicManager.playingMusic.Equals ("theme")) {
			musicManager.playTheme ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (status == 0) {
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
				displayInputName ();
				status = 11;
				curTimer = 0f;
				inputName.transform.position = new Vector3 (inputName.transform.position.x, 1.1f * Screen.height, inputName.transform.position.z);
			} else if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
		} else if (status == 11) {
			if (curTimer >= stopTimer) {
				status = 12;
				inputName.transform.position = new Vector3 (inputName.transform.position.x, 0.5f * Screen.height, inputName.transform.position.z);
			} else {
				inputName.transform.position -= new Vector3 (0, Time.deltaTime / stopTimer * (1.1f - 0.5f) * Screen.height, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 12) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				curTimer = 0;
				status = 131;
			}
		} else if (status == 131 || status == 132) {
			if (curTimer >= stopTimer) {
				if (status == 131) {
					displayHome ();
					status = 0;
				} else {
					displayInputRoomName ();
					inputRoomName.transform.position = new Vector3 (inputRoomName.transform.position.x, 1.1f * Screen.height, inputRoomName.transform.position.z);
					status = 21;
					curTimer = 0;
				}
				inputName.transform.position = new Vector3 (inputName.transform.position.x, 1.1f * Screen.height, inputName.transform.position.z);
			} else {
				inputName.transform.position += new Vector3 (0, Time.deltaTime / stopTimer * (1.1f - 0.5f) * Screen.height, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 21) {
			if (curTimer >= stopTimer) {
				status = 22;
				inputRoomName.transform.position = new Vector3 (inputRoomName.transform.position.x, 0.5f * Screen.height, inputRoomName.transform.position.z);
			} else {
				inputRoomName.transform.position -= new Vector3 (0, Time.deltaTime / stopTimer * (1.1f - 0.5f) * Screen.height, 0);
				curTimer += Time.deltaTime;
			}
		} else if (status == 22) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				curTimer = 0;
				status = 23;
			}
		} else if (status == 23) {
			if (curTimer >= stopTimer) {
				displayInputName ();
				status = 11;
				curTimer = 0;
				inputRoomName.transform.position = new Vector3 (inputRoomName.transform.position.x, 1.1f * Screen.height, inputRoomName.transform.position.z);
			} else {
				inputRoomName.transform.position += new Vector3 (0, Time.deltaTime / stopTimer * (1.1f - 0.5f) * Screen.height, 0);
				curTimer += Time.deltaTime;
			}
		}
	}

	private void displayHome() {
		background.SetActive(true);
		backgroundBlur.SetActive(false);
		inputName.SetActive(false);
		inputRoomName.SetActive(false);
	}

	private void displayInputName() {
		background.SetActive(false);
		backgroundBlur.SetActive(true);
		inputName.SetActive(true);
		inputRoomName.SetActive(false);
	}

	private void displayInputRoomName() {
		background.SetActive(false);
		backgroundBlur.SetActive(true);
		inputName.SetActive(false);
		inputRoomName.SetActive(true);
	}

	public void nameInput(string name){
		PlayerPrefs.SetString ("playerName", name);
	}

	public void NameOk() {
		status = 132;
		curTimer = 0;
		InitGame ();
	}

	public void RoomNameOk() {
		JoinRoom ();
	}

	void InitGame() {
		inLobby = PhotonNetwork.insideLobby;

		PhotonNetwork.offlineMode = false;
		//Automatically sinc scene with master
		PhotonNetwork.automaticallySyncScene = false;

		//Set the room options
		roomopt = new RoomOptions(){IsVisible = true, MaxPlayers = 5};
		roomopt.PublishUserId = true;
		PhotonNetwork.player.NickName = PlayerPrefs.GetString ("playerName", "Player");

		//Connect
		statusText.color = Color.yellow;
		statusText.text = "Connecting...";
		//Connect to the server
		if(!PhotonNetwork.connected){
			PhotonNetwork.ConnectUsingSettings ("v4.2");
		} else if (!inLobby) {
			PhotonNetwork.JoinLobby ();
			Debug.Log ("Joining Lobby");
		} else {
			statusText.color = Color.green;

			statusText.text = "Connected";
		}
	}

	//Updating the room
	void UpdateRoom()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}


	public void JoinRoom(){


		roomopt = new RoomOptions(){IsVisible = false, MaxPlayers = 4};
		roomopt.PublishUserId = true;

		//Get The room list

		PhotonNetwork.offlineMode = false;
		//Joinning Lobby
		if (!PhotonNetwork.connected) {
			infoText.color = Color.red;
			infoText.text = "You're not connected yet";
			PhotonNetwork.ConnectUsingSettings ("v4.2");
			statusText.color = Color.yellow;
			statusText.text = "Connecting...";
		} else if (!inLobby) {
			PhotonNetwork.JoinLobby();
			infoText.color = Color.red;
			infoText.text = "You're not connected yet";
		} 
		else  {
			// Create Room
			if (roomNameField.text == "") {
				infoText.color = Color.red;
				infoText.text = "Name Invalid";
			} 
			else {

				roomName = roomNameField.text;

				PhotonNetwork.JoinOrCreateRoom (roomName, roomopt, TypedLobby.Default);
				infoText.color = Color.green;
				infoText.text = "Joining room...";
			}
		}

	}

	void OnJoinedRoom()
	{	
		//Pindah level
		PhotonNetwork.LoadLevel("CharSelect");
	}
	void OnPhotonJoinRoomFailed(){

		infoText.text = "Can't enter room";

	}

	void OnPhotonJoinOrCreateRoomFailed(){
		infoText.text = "Can't enter room";

	}

	void OnPhotonCreateRoomFailed(){
		infoText.text = "Can't create room";
	}
	void OnJoinedLobby(){
		Debug.Log ("Joined Lobby");

		inLobby = true;
		statusText.color = Color.green;
		statusText.text = "Connected";

	}
	void OnConnectedToMaster(){
		inLobby = true;
		statusText.color = Color.green;
		statusText.text = "Connected";
	}
	void OnDisconnectedFromPhoton(){

		PhotonNetwork.ConnectUsingSettings ("v4.2");
		statusText.color = Color.yellow;
		statusText.text = "Connecting...";
	}
	void OnConnectionFail(DisconnectCause cause) { 	
		statusText.color = Color.red;
		statusText.text = "Failed to connect";
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause){
		statusText.color = Color.red;
		statusText.text = "Failed to connect";
	}
}
