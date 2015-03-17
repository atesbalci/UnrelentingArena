using UnityEngine;
using System.Collections;

public enum StageState {
    normal, unstable, damaging
}

public class StageScript : MonoBehaviour {
    public float damage = 20;
    public Color warningColor = new Color(255/255.0F,102/255.0F,0/255.0F,255/255.0F);
    public Color damagingColor = Color.red;
    public float blinkSpeed = 4;
    public float unstableTimer = 3;

    private Renderer rend;
    private bool blinking;
    private float remainingUnstableTimer;
    private Color defaultColor;
    private Color color;

    private StageState _state;
    public StageState state {
        get {
            return _state;
        }
        set {
            _state = value;
            if (state == StageState.normal)
                GetComponent<Renderer>().material.color = defaultColor;
            else if (state == StageState.damaging)
                GetComponent<Renderer>().material.color = damagingColor;
            else if (state == StageState.unstable)
                remainingUnstableTimer = unstableTimer;
        }
    }

    void Start() {
        rend = GetComponent<Renderer>();
        color = rend.material.color;
        defaultColor = color;
        state = StageState.normal;
        blinking = false;
    }

    void Update() {
        if (state == StageState.unstable) {
            if (blinking) {
                color = Color.Lerp(color, warningColor, blinkSpeed * Time.deltaTime);
                if (checkSimilarity(color, warningColor))
                    blinking = false;
            } else {
                color = Color.Lerp(color, defaultColor, blinkSpeed * Time.deltaTime);
                if (checkSimilarity(color, defaultColor))
                    blinking = true;
            }
            rend.material.color = color;
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

    void OnCollisionStay(Collision collider) {
        if (state == StageState.damaging) {
            if (Network.isServer) {
                if (collider.gameObject.tag == "Player") {
                    collider.gameObject.GetComponent<PlayerScript>().player.Damage(damage * Time.deltaTime, null);
                }
            }
        }
    }
}
