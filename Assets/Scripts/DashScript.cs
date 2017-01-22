using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashScript : MonoBehaviour {
	private Player player;
	private float damage;
	private List<Player> affectedPlayers;
	private ParticleSystem partSys;

	void Awake() {
		player = GetComponentInParent<PlayerScript>().player;
		damage = 10;
		partSys = GetComponentInChildren<ParticleSystem>();
		partSys.startColor = player.color;
		partSys.Stop();
		partSys.Play();
		partSys.Stop();
	}

	void OnEnable() {
		affectedPlayers = new List<Player>();
		partSys.Stop();
		partSys.Play();
		partSys.Stop();
	}

	void Update() {
		if (!partSys.IsAlive())
			gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider col) {
		if (Network.isServer && partSys.isPlaying) {
			if (col.gameObject == player.gameObject)
				return;
			PlayerScript ps = col.GetComponent<PlayerScript>();
			if (ps != null) {
				foreach (Player p in affectedPlayers)
					if (ps.player == p)
						return;
				ps.player.Damage(damage, player);
				ps.Knockback(ps.transform.position - new Vector3(transform.position.x, 0, transform.position.z), 10, 10);
				affectedPlayers.Add(ps.player);
			}
		}
	}
}
