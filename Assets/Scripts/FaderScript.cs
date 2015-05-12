﻿using UnityEngine;
using System.Collections;

public class FaderScript : MonoBehaviour {
    public float fadeSpeed { get; set; }
    private Material skin;
    private float remaining;

    void Start() {
        skin = GetComponentInChildren<SkinnedMeshRenderer>().material;
        skin.SetFloat("_Mode", 4f);
        skin.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        skin.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        skin.SetInt("_ZWrite", 0);
        skin.DisableKeyword("_ALPHATEST_ON");
        skin.EnableKeyword("_ALPHABLEND_ON");
        skin.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        skin.renderQueue = 3000;
        remaining = fadeSpeed;
    }

    void Update() {
        remaining -= Time.deltaTime;
        if (remaining <= 0) {
            Destroy(gameObject);
            return;
        }
        skin.SetColor("_Color", new Color(skin.color.r, skin.color.g, skin.color.b, remaining / fadeSpeed));
    }
}