using UnityEngine;
using System.Collections;

public class FaderScript : MonoBehaviour {
    public float fadeSpeed;
    private Material[] skins;
    private float remaining;

    void Start() {
        skins = GetComponentInChildren<Renderer>().materials;
        foreach (Material skin in skins) {
            skin.SetFloat("_Mode", 4f);
            skin.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            skin.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            skin.SetInt("_ZWrite", 0);
            skin.DisableKeyword("_ALPHATEST_ON");
            skin.EnableKeyword("_ALPHABLEND_ON");
            skin.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            skin.renderQueue = 3000;
        }
        remaining = fadeSpeed;
    }

    void Update() {
        remaining -= Time.deltaTime;
        if (remaining <= 0) {
            Destroy(gameObject);
            return;
        }
        foreach (Material skin in skins)
            skin.color = new Color(skin.color.r, skin.color.g, skin.color.b, remaining / fadeSpeed);
    }
}
