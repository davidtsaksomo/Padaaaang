using UnityEngine;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour {

	public void LoadTitle(){
		Application.LoadLevelAsync ("Title");
	}

	public void LoadRoomName(){
		Application.LoadLevelAsync ("RoomName");

	}
	public void LoadWaitingRoom(){
		Application.LoadLevelAsync ("WaitingRoom");

	}
	public void LoadGame(){
		Application.LoadLevelAsync ("Game");
	}
	public void Quit(){
		Application.Quit ();
	}
}
