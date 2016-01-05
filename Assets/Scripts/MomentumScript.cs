using UnityEngine;
using System.Collections;

public class MomentumScript : MonoBehaviour {
	private Material[] trail;
	private float time;
	private Player player;

	void Start() {
		trail = GetComponent<TrailRenderer>().materials;
		player = GetComponentInParent<PlayerScript>().player;
		time = -1;
	}

	public void GainMomentum(bool gain) {
		if (time <= 0 && gain)
			time = 2;
		else if (!gain)
			time = 0;
	}

	void Update() {
		bool colorDependent = false;
		foreach (Material m in trail) {
			m.SetColor("_TintColor", time > 0 ? (!colorDependent ? player.color : Color.white) : Color.Lerp(m.GetColor("_TintColor"), Color.clear, 0.1f));
			colorDependent = true;
		}
		if (time > 0) {
			time -= Time.deltaTime;
			if (time <= 0)
				player.modifier = ComboModifier.Composure;
		}
	}
}
