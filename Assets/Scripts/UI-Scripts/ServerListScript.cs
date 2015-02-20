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
        float scroll = rectTransform.offsetMax.y;
        if (refresh <= 0) {
            refresh = 1;
            rectTransform.sizeDelta = new Vector2(GetComponentInParent<RectTransform>().rect.width, 0);
            foreach (Button child in GetComponentsInChildren<Button>()) {
                GameObject.Destroy(child.gameObject);
            }
            int y = 0;
            if (Camera.main.GetComponent<GameManager>().hostData != null) {
                foreach (HostData hd in Camera.main.GetComponent<GameManager>().hostData) {
                    //for (int i = 0; i < 30; i++) {
                    GameObject hostData = Instantiate(Resources.Load("UI-Elements/HostData")) as GameObject;
                    hostData.transform.SetParent(gameObject.transform);
                    hostData.GetComponent<HostDataScript>().hostData = hd;
                    hostData.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y - 15);
                    y -= 30;
                }
                if (-y > rectTransform.rect.height)
                    rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, -y);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, scroll);
            }
        }
    }
}
