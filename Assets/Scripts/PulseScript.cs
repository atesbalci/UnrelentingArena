﻿using UnityEngine;
using System.Collections;

public class PulseScript : MonoBehaviour {
    public NetworkView view;
    public Player player { get; set; }

    private const float MAX_DURATION = 1;
    private float state;
    private float damage;
    private Material material;

    void Start() {
        player = GameManager.instance.playerList[view.owner].currentPlayer;
        transform.eulerAngles = new Vector3(270, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        state = MAX_DURATION;
        damage = 20;
        material = GetComponent<Renderer>().material;
        material.SetColor("_EmissionColor", player.color);
    }

    void Update() {
        material.color = new Color(material.color.r, material.color.g, material.color.b, state / MAX_DURATION);
        float scale = 3 - state / MAX_DURATION;
        transform.localScale = new Vector3(scale, scale, 1);
        state -= Time.deltaTime;
        if (Network.isServer && state <= 0)
            Network.Destroy(gameObject);
    }

    void OnTriggerStay(Collider col) {
        if (Network.isServer && state >= MAX_DURATION) {
            if (col.gameObject == player.gameObject)
                return;
            PlayerScript ps = col.GetComponent<PlayerScript>();
            if (ps != null) {
                ps.player.Damage(damage, player);
                ps.ApplyKnockback(ps.transform.position - transform.position, 10, 10);
            }
        }
    }
}