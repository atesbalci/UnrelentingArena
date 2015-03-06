using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerListScript : MonoBehaviour {
    private RectTransform rectTransform;
    private float refresh;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        refresh = 0;
    }

    void Update() {
        refresh -= Time.deltaTime;
        if (refresh <= 0) {
            refresh = 1;
            rectTransform.sizeDelta = new Vector2(GetComponentInParent<RectTransform>().rect.width, 0);
            foreach (Button child in GetComponentsInChildren<Button>()) {
                GameObject.Destroy(child.gameObject);
            }
            HostData[] hostData = Camera.main.GetComponent<GameManager>().hostData;
            if (hostData != null) {
                foreach (HostData hd in hostData) {
                    GameObject host = Instantiate(Resources.Load("UI-Elements/HostData")) as GameObject;
                    host.transform.SetParent(gameObject.transform);
                    host.GetComponent<HostDataScript>().hostData = hd;
                }
            }
        }
    }
}
