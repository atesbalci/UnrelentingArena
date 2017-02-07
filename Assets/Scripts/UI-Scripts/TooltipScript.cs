using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipScript : MonoBehaviour {
    public Text text;
	public Image image;

	private float currentAlpha;
	private float defaultAlpha;

	void Awake() {
		defaultAlpha = image.color.a;
		currentAlpha = 0;
	}

	void OnEnable() {
		Update();
	}

	void OnDisable() {
		currentAlpha = 0;
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
	}

    void Update() {
		currentAlpha = Mathf.Lerp(currentAlpha, defaultAlpha, Time.deltaTime * 10);
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
		text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 1, Time.deltaTime * 10));
        transform.position = new Vector3(Input.mousePosition.x + 5, Input.mousePosition.y - 5, Input.mousePosition.z);
    }
}
