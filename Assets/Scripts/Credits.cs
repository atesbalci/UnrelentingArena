using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {
	private Image back;
	private Text text;
	private RectTransform textTransform;
	private bool fading;

	private float _alpha;
	public float alpha {
		get {
			return _alpha;
		}
		set {
			_alpha = value;
			back.color = new Color(back.color.r, back.color.g, back.color.b, alpha);
			text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
		}
	}

	void Awake() {
		back = GetComponent<Image>();
		text = GetComponentInChildren<Text>();
		textTransform = text.GetComponent<RectTransform>();
	}

	void OnEnable() {
		alpha = 1;
		fading = false;
		textTransform.anchoredPosition = Vector2.zero;
	}

	public void Fade() {
		fading = true;
	}

	public void ShowCredits() {
		gameObject.SetActive(true);
	}

	void Update() {
		Vector2 target = Vector2.up * (Screen.height + textTransform.rect.height);
		textTransform.anchoredPosition = Vector2.MoveTowards(textTransform.anchoredPosition, target, Time.deltaTime * 100);
		if (Vector2.Distance(target, textTransform.anchoredPosition) < 0.1f)
			Fade();
		if (fading)
			alpha = Mathf.MoveTowards(alpha, 0.0f, Time.deltaTime);
		if (alpha <= 0.001f)
			gameObject.SetActive(false);
	}
}
