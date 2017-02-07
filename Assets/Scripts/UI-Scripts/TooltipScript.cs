using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipScript : MonoBehaviour {
    public Text text;
	public Image image;

	private float currentAlpha;
	private float defaultAlpha;
	private bool _active;

	void Awake() {
		defaultAlpha = image.color.a;
		currentAlpha = 0;
		active = gameObject.activeInHierarchy;
	}

	public bool active {
		get {
			return _active;
		}
		set {
			_active = value;
			if(active) {
				gameObject.SetActive(true);
				Update();
			} else {
				text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
			}
		}
	}

    void Update() {
		currentAlpha = Mathf.Lerp(currentAlpha, active ? defaultAlpha : 0, Time.deltaTime * 10);
		Debug.Log(currentAlpha);
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
		text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, active ? 1 : 0, Time.deltaTime * 10));
		if(active)
			transform.position = new Vector3(Input.mousePosition.x + 5, Input.mousePosition.y - 5, Input.mousePosition.z);
		if (!active && currentAlpha < 0.01f)
			gameObject.SetActive(false);
    }
}
