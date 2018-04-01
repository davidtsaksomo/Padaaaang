using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSave : MonoBehaviour {
	[SerializeField] InputField input_name;
	void Start () {
		PlayerPrefs.DeleteKey ("roomname");
		input_name.GetComponent<InputField>().text = PlayerPrefs.GetString ("playerName", "Player");
	}
	
	public void nameInput(string name){
		PlayerPrefs.SetString ("playerName", name);
	}
}