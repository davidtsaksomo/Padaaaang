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
	private int status;

	//Reference to info text
	[SerializeField] Text infoText;
	[SerializeField] Text statusText;
	[SerializeField] InputField input_roomname;
	//Properties for room
	private string roomName;
	private RoomInfo[] roomsList;
	private RoomOptions roomopt;

	bool inLobby = false;

	// Use this for initialization
	void Start () {
		background.SetActive(true);
		backgroundBlur.SetActive(false);
		inputName.SetActive(false);
		inputRoomName.SetActive(false);
		PlayerPrefs.DeleteKey ("roomname");
		nameField.GetComponent<InputField>().text = PlayerPrefs.GetString ("playerName", "Player");
		status = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began && status == 0) {
			background.SetActive(false);
			backgroundBlur.SetActive(true);
			inputName.SetActive(true);
			inputRoomName.SetActive(false);
			status = 1;
		}
	}

	public void nameInput(string name){
		PlayerPrefs.SetString ("playerName", name);
	}

	public void NameOk() {
		background.SetActive(false);
		backgroundBlur.SetActive(true);
		inputName.SetActive(false);
		inputRoomName.SetActive(true);
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
			if (input_roomname.text == "") {
				infoText.color = Color.red;
				infoText.text = "Name Invalid";
			} 
			else {

				roomName = input_roomname.text;

				PhotonNetwork.JoinOrCreateRoom (roomName, roomopt, TypedLobby.Default);
				infoText.color = Color.green;
				infoText.text = "Joining room...";
			}
		}

	}

	void OnJoinedRoom()
	{	
		//Pindah level
		PhotonNetwork.LoadLevel("WaitingRoom");
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
