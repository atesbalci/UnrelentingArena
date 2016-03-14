using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VSScript : MonoBehaviour {
    public Image overlay;
    public Image overlay2;

    void OnEnable() {
        overlay.gameObject.SetActive(false);
    }

    void Update() {
        if (overlay.IsActive()) {
            overlay2.transform.localScale = new Vector3(overlay2.transform.localScale.x + Time.deltaTime, overlay2.transform.localScale.y + Time.deltaTime, overlay2.transform.localScale.z);
            if (overlay2.transform.localScale.x > 1.5f) {
                overlay2.transform.localScale = new Vector3(0.5f, 0.5f, overlay2.transform.localScale.z);
            }
        } else if (!GameManager.instance.locked && GameManager.instance.headCount <= 2) {
            overlay.color = GameManager.instance.playerData.currentPlayer.color;
            overlay.gameObject.SetActive(true);
        }
    }
}
