using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KucingSpawner : MonoBehaviour {

	bool adaKucing = false;
	public float kucingStart;
	public float kucingRepeat;
	[SerializeField] GameObject kucing;
	[SerializeField] Transform  canvas;
	[SerializeField] Sprite  normal;
	[SerializeField] Sprite  terusir;
	[SerializeField] GenerateMakanan  generateMakanan;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("AmbilMakanan", kucingStart, kucingRepeat);
		InvokeRepeating ("SpawnKucing", kucingStart, kucingRepeat);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnKucing(){
		if (adaKucing)
			return;
		int a = Random.Range (0, 4);

		if (a == 0) {
			adaKucing = true;
			kucing.SetActive (true);
		}
	}

	void AmbilMakanan(){
		if (!adaKucing)
			return;
		int i = Random.Range (0, generateMakanan.foods.Length);
		generateMakanan.foods [i].GetComponent<FoodCoolDown> ().emptyFood ();
	}
	void ClearKucing(){
		kucing.SetActive (false);
		kucing.GetComponent<Image> ().sprite = normal;

	}
	[PunRPC]
	void PanggilSatpam(int chefID) {
		if (PhotonNetwork.player.ID == chefID) {
			if (!adaKucing) {
				Instantiate (Resources.Load ("GaadaKucingText"), canvas);
			} else {
				adaKucing = false;
				kucing.GetComponent<Image> ().sprite = terusir;
				Instantiate (Resources.Load ("PergiKau"), canvas);
				Invoke ("ClearKucing", 2f);
			}

		}
	}
}
