using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour {

	float widthMax;
	public float percentage=1f;
	InGameMusic inGameMusic;

	void Start(){
		widthMax = GetComponent<RectTransform> ().sizeDelta.x;
		inGameMusic = GameObject.Find ("In-Game Music").GetComponent<InGameMusic> ();
		inGameMusic.init ();
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
		if (terbesar) {
			inGameMusic.startMusic ();
		}
	}
	public void setValue(float value){
		Color color = new Color();
		color.r = 1f - Mathf.Max((value-0.5f),0f)*2;
		color.g = Mathf.Min((value),0.5f)*2;
		color.a = 1f;
		color.b = 0;
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (widthMax * value, GetComponent<RectTransform> ().sizeDelta.y);
		GetComponent<Image> ().color = color;
		percentage = GetComponent<RectTransform> ().sizeDelta.x / widthMax;
	}
}
