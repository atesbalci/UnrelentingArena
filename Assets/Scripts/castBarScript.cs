using UnityEngine;
using System.Collections;

public class CastBarScript : MonoBehaviour {
    private GameObject castBar;

	void Start () {
        castBar = GameObject.FindGameObjectWithTag("CastBar");
	}
	
	void Update () {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().player;
        Channel channel = player.getChannel();
        if (channel != null) {
            bool casting = channel.skill != null;
            castBar.SetActive(casting);
        } else
            castBar.SetActive(false);
	}
}
