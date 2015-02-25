using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameDisplayScript : MonoBehaviour {
    private Player player;

	void Start () {
        player = GetComponentInParent<PlayerScript>().player;
	}
	
	void Update () {
        GetComponent<Text>().text = player.name;
	}
}
