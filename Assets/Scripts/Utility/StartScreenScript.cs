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
	public Image icon;
	public Sprite[] connectingSprite;
	public Sprite[] connectedSprite;
	public Sprite[] notConnectedSprite;
	public Sprite[] loadingSprite;
	public int changeIcon;
	public float stopTimer;
	private float curTimer;
	MusicManager musicManager;
	/**
	 * Status:
	 * home screen: 0
	 * input name transition down: 11
	 * input name stop: 12
	 * input name transition up, go to home: 131
	 * input name transition up, go to input room name: 132
	 * input room name transition down: 21
	 * input room name stop: 22
	 * input room name transition up: 23
	 **/
	private int status;

	//Reference to info text
	[SerializeField] Text infoText;
	[SerializeField] Text statusText;
	//Properties for room
	private string roomName;
	private RoomOptions roomopt;
	/**
	 * Room status:
	 * 0: connecting
	 * 1: connected
	 * 2: not connected
	 * 3: loading
	 **/
	private int roomStatus = 0;
	private int iconIdx = 0;
	private int changeIconCounter = 0;
	private InGameMusic ingame;

	bool inLobby = false;

	// Use this for initialization
	void Start () {
		displayHome ();
		PlayerPrefs.DeleteKey ("roomname");
		nameField.GetComponent<InputField>().text = PlayerPrefs.GetString ("playerName", "Player");
		status = 0;
		musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		ingame = GameObject.Find ("In-Game Music").GetComponent<InGameMusic> ();
		if (ingame != null) {
			ingame.stopMusic ();
		}
		if (!musicManager.playingMusic.Equals ("theme")) {
			musicManager.setVolume (1f);
			musicManager.playTheme ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (status == 0) {
			bool click = false;
			#if UNITY_EDITOR
				click = Input.GetMouseButtonDown(0);
			#endif
			if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) || click) {
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
			InitGame ();
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
		if (changeIconCounter == changeIcon) {
			if (roomStatus == 0) {
				iconIdx = (iconIdx + 1) % connectingSprite.Length;
				icon.sprite = connectingSprite [iconIdx];
			} else if (roomStatus == 1) {
				iconIdx = (iconIdx + 1) % connectedSprite.Length;
				icon.sprite = connectedSprite [iconIdx];
			} else if (roomStatus == 2) {
				iconIdx = (iconIdx + 1) % notConnectedSprite.Length;
				icon.sprite = notConnectedSprite [iconIdx];
			} else if (roomStatus == 3) {
				iconIdx = (iconIdx + 1) % loadingSprite.Length;
				icon.sprite = loadingSprite [iconIdx];
			}
			changeIconCounter = 0;
		} else {
			changeIconCounter++;
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
		roomopt.PlayerTtl = 0;
		roomopt.EmptyRoomTtl = 0;
		roomopt.PublishUserId = true;
		PhotonNetwork.player.NickName = PlayerPrefs.GetString ("playerName", "Player");
		PhotonNetwork.player.SetTeam (PunTeams.Team.none);

		//Connect
		statusText.color = Color.yellow;
		statusText.text = "Connecting...";
		roomStatus = 3;
		//Connect to the server
		if(!PhotonNetwork.connected){
			PhotonNetwork.ConnectUsingSettings ("v4.2");
		} else if (!inLobby) {
			PhotonNetwork.JoinLobby ();
			Debug.Log ("Joining Lobby");
		} else {
			statusText.color = Color.green;

			statusText.text = "Connected";
			roomStatus = 1;
		}
	}

	public void JoinRoom(){


		roomopt = new RoomOptions(){IsVisible = false, MaxPlayers = 6};
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
			roomStatus = 3;
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

				roomStatus = 0;
				roomName = roomNameField.text;

				PhotonNetwork.JoinOrCreateRoom (roomName, roomopt, TypedLobby.Default);
				infoText.color = Color.green;
				infoText.text = "Joining room...";
				if (musicManager.playingMusic.Equals("theme")) {
					musicManager.stopTheme ();
				}
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
		roomStatus = 2;
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause){
		statusText.color = Color.red;
		statusText.text = "Failed to connect";
		roomStatus = 2;
	}
}
