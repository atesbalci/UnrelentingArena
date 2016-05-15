using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextAdapter : MonoBehaviour {
	public Text source;

	private Text text;

	void Start () {
		text = GetComponent<Text>();
		if (source == null)
			source = text;
	}
	
	void Update () {
		text.fontSize = source.cachedTextGenerator.fontSizeUsedForBestFit;
	}
}
