using UnityEngine;
using System.Collections;

public class CharacterShowcaseScript : MonoBehaviour {
    public SkinnedMeshRenderer rend;
    public Color[] colors;

    private int currentIndex = 0;
    private int nextIndex;
    private float changeColourTime = 2.0f;
    private float timer = 0.0f;

    void Start() {
        nextIndex = (currentIndex + 1) % colors.Length;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer > changeColourTime) {
            currentIndex = (currentIndex + 1) % colors.Length;
            nextIndex = (currentIndex + 1) % colors.Length;
            timer = 0.0f;

        }
        rend.materials[0].SetColor("_EmissionColor", Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime) * Mathf.LinearToGammaSpace(4f));
    }
}