using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterShowcaseScript : MonoBehaviour {
	public Renderer[] additionalRenderers;
    public Color[] colors;

	public Color nextColor { get; set; }

	private Color currentColor;
	private Renderer[] renderers;
	private int currentIndex = 0;
    private int nextIndex;
    private float changeColourTime = 2.0f;
    private float timer = 0.0f;

    void Start() {
		SkinnedMeshRenderer[] body = GetComponentsInChildren<SkinnedMeshRenderer>();
		List<Renderer> rendererList = new List<Renderer>();
		foreach(SkinnedMeshRenderer smr in body)
			rendererList.Add(smr);
		foreach (Renderer r in additionalRenderers)
			rendererList.Add(r);
		renderers = rendererList.ToArray();
        nextIndex = (currentIndex + 1) % colors.Length;
		currentColor = Color.white;
		nextColor = currentColor;
    }

    void Update() {
		//      timer += Time.deltaTime;

		//      if (timer > changeColourTime) {
		//          currentIndex = (currentIndex + 1) % colors.Length;
		//          nextIndex = (currentIndex + 1) % colors.Length;
		//          timer = 0.0f;

		//      }
		//Color newColor = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime);
		Color newColor = Color.Lerp(currentColor, nextColor, Time.deltaTime * 5);
		foreach (Renderer rend in renderers)
			rend.materials[0].SetColor("_EmissionColor", newColor * Mathf.LinearToGammaSpace(4f));
		currentColor = newColor;
    }

	public void SetNextColor(Image image) {
		nextColor = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
	}

	public void ResetColor() {
		nextColor = Color.white;
	}
}