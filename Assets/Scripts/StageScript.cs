using UnityEngine;
using System.Collections;

public enum StageState {
    normal, unstable, damaging
}

public class StageScript : MonoBehaviour {
    public float damage = 20;
    public Color warningColor = Color.red;
    public Color damagingColor = new Color(0, 0, 0, 0.6f);
    public float blinkSpeed = 4;
    public float unstableTimer = 3;

    private bool blinking;
    private float remainingUnstableTimer;
    private Color defaultColor;

    private StageState _state;
    public StageState state {
        get {
            return _state;
        }
        set {
            _state = value;
            if (state == StageState.normal)
                renderer.material.color = defaultColor;
            else if (state == StageState.damaging)
                renderer.material.color = damagingColor;
            else if (state == StageState.unstable)
                remainingUnstableTimer = unstableTimer;
        }
    }

    void Start() {
        defaultColor = renderer.material.color;
        state = StageState.normal;
        blinking = false;
    }

    void Update() {
        if (state == StageState.unstable) {
            if (blinking) {
                renderer.material.color = Color.Lerp(renderer.material.color, warningColor, blinkSpeed * Time.deltaTime);
                if (checkSimilarity(renderer.material.color, warningColor))
                    blinking = false;
            } else {
                renderer.material.color = Color.Lerp(renderer.material.color, defaultColor, blinkSpeed * Time.deltaTime);
                if (checkSimilarity(renderer.material.color, defaultColor))
                    blinking = true;
            }
            remainingUnstableTimer -= Time.deltaTime;
            if (remainingUnstableTimer <= 0)
                state = StageState.damaging;
        }
    }

    public bool checkSimilarity(Color c1, Color c2) {
        const float SIMILARITY = 0.1f;
        return Mathf.Abs(c1.r - c2.r) < SIMILARITY && 
            Mathf.Abs(c1.g - c2.g) < SIMILARITY && 
            Mathf.Abs(c1.b - c2.b) < SIMILARITY && 
            Mathf.Abs(c1.a - c2.a) < SIMILARITY;
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
