using UnityEngine;
using System.Collections;

public class Scatter : SkillScript {
    private const float SPEED = 30;

    private Transform[] spheres;

    public Scatter()
        : base() {
        skillType = SkillType.Scatter;
    }

    public override void Start() {
        base.Start();
        spheres = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            spheres[i] = transform.GetChild(i);
            spheres[i].GetComponent<TrailRenderer>().material.SetColor("_TintColor", player.color);
        }
        spheres[0].Rotate(Vector3.up, -20);
        spheres[2].Rotate(Vector3.up, 20);

    }

    public override void Update() {
        foreach(Transform sphere in spheres) {
            sphere.Translate(Vector3.forward * Time.deltaTime * SPEED);
        }
        if (Vector3.Distance(spheres[1].position, transform.position) >= preset.range)
            Destroy(gameObject);
    }
}
