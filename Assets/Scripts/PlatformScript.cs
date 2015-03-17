using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {
    private int currentIndex = 0;
    private int nextIndex;
    private float changeColourTime = 2.0f;
    private float timer = 0.0f;
    private Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };

    void Start() {
        if (colors == null || colors.Length < 2)
            Debug.Log("Need to setup colors array in inspector");

        nextIndex = (currentIndex + 1) % colors.Length;
    }

    void Update() {
        timer += Time.deltaTime;

        if (timer > changeColourTime) {
            currentIndex = (currentIndex + 1) % colors.Length;
            nextIndex = (currentIndex + 1) % colors.Length;
            timer = 0.0f;

        }
        GetComponent<Renderer>().material.color = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime);
    }
}