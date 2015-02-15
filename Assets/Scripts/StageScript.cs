using UnityEngine;
using System.Collections;

public enum StageState {
    normal, unstable, damaging
}

public class StageScript : MonoBehaviour {
    public float damage = 20;
    public Color warningColor = Color.red;
    public Color damagingColor = new Color(0, 0, 0, 0.6f);
    public float fadeSpeed = 4;
    public float unstableTimer;

    private bool fading;
    private float remainingUnstableTimer;

    private StageState _state;
    public StageState state {
        get {
            return _state;
        }
        set {
            _state = value;
            if (state == StageState.normal)
                renderer.material.color = Color.clear;
            else if (state == StageState.damaging)
                renderer.material.color = damagingColor;
            else if (state == StageState.unstable)
                remainingUnstableTimer = unstableTimer;
        }
    }

    void Start() {
        state = StageState.normal;
        fading = false;
    }

    void Update() {
        if (state == StageState.unstable) {
            if (fading) {
                renderer.material.color = Color.Lerp(renderer.material.color, Color.clear, fadeSpeed * Time.deltaTime);
                if (renderer.material.color.a <= 0.1f)
                    fading = false;
            } else {
                renderer.material.color = Color.Lerp(renderer.material.color, warningColor, fadeSpeed * Time.deltaTime);
                if (renderer.material.color.a >= 0.9f)
                    fading = true;
            }
            remainingUnstableTimer -= Time.deltaTime;
            if (remainingUnstableTimer <= 0)
                state = StageState.damaging;
        }
    }

    void OnTriggerStay(Collider collider) {
        if (state == StageState.damaging) {
            if (Network.isServer) {
                if (collider.gameObject.tag == "Player") {
                    collider.gameObject.GetComponent<PlayerScript>().player.Damage(damage * Time.deltaTime);
                }
            }
        }
    }
}
