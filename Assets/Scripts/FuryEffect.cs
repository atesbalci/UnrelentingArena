using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuryEffect : MonoBehaviour {
	public DashScript dash;

	private float time;
	private Player player;
	private Material[] materials;

	void OnEnable() {
		GetComponent<Animator>().Play("Fury", -1, 0);
		time = 2;
	}

	void Start() {
		player = GetComponentInParent<PlayerScript>().player;
		List<Material> mats = new List<Material>();
		foreach (Renderer rend in GetComponentsInChildren<Renderer>())
			mats.Add(rend.material);
		materials = mats.ToArray();
	}

	void Update() {
		foreach (Material mat in materials)
			mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Min(1, time));
		if (time <= 0) {
			gameObject.SetActive(false);
			player.modifier = ComboModifier.Composure;
		}
		time -= Time.deltaTime;
	}

	void OnDisable() {
		if (player != null)
			if (player.modifier == ComboModifier.Momentum) {
				dash.gameObject.SetActive(true);
				player.modifier = ComboModifier.Composure;
			}
	}
}