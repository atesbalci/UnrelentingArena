using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterShowcaseScript : MonoBehaviour {
	public Renderer[] additionalRenderers;
    public Color[] colors;
	public Transform head;

	public Color nextColor { get; set; }

	private Color currentColor;
	private Renderer[] renderers;
	private int currentIndex = 0;
    private int nextIndex;
    private float changeColourTime = 2.0f;
    private float timer = 0.0f;
	private Quaternion headRotation;
	private Plane lookPlane;

    void Start() {
		headRotation = head.transform.rotation;
		lookPlane = new Plane(new Vector3(0, 0, 1), new Vector3(0, 0, 10));
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

	void LateUpdate() {
		float hitdst = 0;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		lookPlane.Raycast(ray, out hitdst);
        head.LookAt(ray.GetPoint(hitdst));
		head.rotation = Quaternion.Lerp(headRotation, head.rotation, 10 * Time.deltaTime);
		headRotation = head.rotation;
	}
}