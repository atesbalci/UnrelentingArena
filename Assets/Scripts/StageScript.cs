using UnityEngine;
using System.Collections;

public enum StageState {
    normal, unstable, damaging
}

public class StageScript : MonoBehaviour {
    public float damage = 20;
    public Color damagingColor = Color.red;
    public float unstableTimer = 3;
    public Color defaultColor = Color.white;

    private Renderer rend;
    private float remainingUnstableTimer;
    private Color color;

    private StageState _state;
    public StageState state {
        get {
            return _state;
        }
        set {
            _state = value;
            if (state == StageState.unstable)
                remainingUnstableTimer = unstableTimer;
            else if (state == StageState.damaging) {
                color = damagingColor;
                rend.material.SetFloat("_Cutoff", 0);
            } else if (state == StageState.normal) {
                color = defaultColor;
                rend.material.SetFloat("_Cutoff", 0);
            }
        }
    }

    void Start() {
        rend = GetComponent<Renderer>();
        color = defaultColor;
        state = StageState.normal;
    }

    void Update() {
        if (state == StageState.unstable) {
            remainingUnstableTimer -= Time.deltaTime;
            rend.material.SetFloat("_Cutoff", 1 - remainingUnstableTimer / unstableTimer);
            if (remainingUnstableTimer <= 0)
                state = StageState.damaging;
        }
        rend.material.SetColor("_Color", Color.Lerp(rend.material.GetColor("_Color"), color, 4 * Time.deltaTime));
    }

    public bool checkSimilarity(Color c1, Color c2) {
        const float SIMILARITY = 0.2f;
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
