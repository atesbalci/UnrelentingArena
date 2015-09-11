using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VSScript : MonoBehaviour {
    public Image overlay;
    public Image overlay2;

    //private GameObject overlay2Parent;

    void OnEnable() {
        overlay.color = GameManager.instance.playerData.currentPlayer.color;
        //overlay2.color = GameManager.instance.playerData.currentPlayer.color;
        //overlay2Parent = overlay2.gameObject.transform.parent.gameObject;
    }

    void Update() {
        //overlay2Parent.transform.localScale = new Vector3(overlay2Parent.transform.localScale.x + Time.deltaTime, overlay2Parent.transform.localScale.y + Time.deltaTime, overlay2Parent.transform.localScale.z);
        //overlay2.color = new Color(overlay2.color.r, overlay2.color.g, overlay2.color.b, overlay2.color.a - Time.deltaTime / 4);
        //if (overlay2Parent.transform.localScale.x > 2) {
        //    overlay2Parent.transform.localScale = new Vector3(1, 1, overlay2Parent.transform.localScale.z);
        //    overlay2.color = new Color(overlay2.color.r, overlay2.color.g, overlay2.color.b, 0.25f);            
        //}
        overlay2.transform.localScale = new Vector3(overlay2.transform.localScale.x + Time.deltaTime, overlay2.transform.localScale.y + Time.deltaTime, overlay2.transform.localScale.z);
        if (overlay2.transform.localScale.x > 1.5f) {
            overlay2.transform.localScale = new Vector3(0.5f, 0.5f, overlay2.transform.localScale.z);
        }
    }
}
