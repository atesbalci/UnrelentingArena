using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {
    private int currentIndex = 0;
    private int nextIndex;
    private float changeColourTime = 2.0f;
    private float timer = 0.0f;
    private Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };
    private Material material;

    void Start() {
        nextIndex = (currentIndex + 1) % colors.Length;
        material = GetComponent<Renderer>().material;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer > changeColourTime) {
            currentIndex = (currentIndex + 1) % colors.Length;
            nextIndex = (currentIndex + 1) % colors.Length;
            timer = 0.0f;

        }
        material.SetColor("_EmissionColor", Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime));
    }
}