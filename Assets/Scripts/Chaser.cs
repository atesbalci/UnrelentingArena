using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour {
    public Transform target;

    private TrailRenderer trail;
    private SpriteRenderer sprite;

    void Start() {
        trail = GetComponent<TrailRenderer>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position, 10 * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
            Destroy(gameObject);
	}

    public Color color {
        set {
            GetComponent<TrailRenderer>().material.SetColor("_TintColor", value);
            GetComponent<SpriteRenderer>().color = value;
        }
    }
}
