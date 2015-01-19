using UnityEngine;
using System.Collections;

public class Skill {
    private float range;
    private float speed;
    private string prefab;

    public Skill() {
        range = 10f;
        speed = 1.5f;
        prefab = "";
    }

    public float getRange() {
        return range;
    }

    public void setRange(float range) {
        this.range = range;
    }

    public float getSpeed() {
        return speed;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }

    public string getPrefab() {
        return prefab;
    }

    public void setPrefab(string prefab) {
        this.prefab = prefab;
    }
}
