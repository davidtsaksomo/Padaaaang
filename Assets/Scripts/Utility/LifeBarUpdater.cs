using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarUpdater : MonoBehaviour {
	PhotonView photonView;
	[SerializeField] LifeBar lifebar;
	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	[PunRPC]
	void UpdateBar (float value) {
		lifebar.setValue (value);
	}
}
