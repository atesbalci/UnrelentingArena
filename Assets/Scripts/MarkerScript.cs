using UnityEngine;
using System.Collections;

public class MarkerScript : MonoBehaviour {
    public float duration;

    private SpriteRenderer sprite;
    private float marked;
    private Plane plane;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        plane = new Plane(Vector3.up, transform.position); 
        marked = 0;
    }

    void Update() {
        if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Move])) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist;
            if (plane.Raycast(ray, out hitdist)) {
                transform.position = ray.GetPoint(hitdist);
                marked = duration;
            }
        }
        if (marked > 0) {
            float lerp = Mathf.Lerp(0, 1, marked / duration);
            sprite.color = new Color(1, 1, 1, Mathf.Lerp(0.05f, 2, marked / duration));
            transform.localScale = new Vector3(1 - lerp, 1 - lerp);
            marked -= Time.deltaTime;
        } else {
            sprite.color = Color.clear;
        }
    }
}
