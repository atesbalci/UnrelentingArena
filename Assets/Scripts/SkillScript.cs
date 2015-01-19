using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    private float remainingDistance = 0;
    private float speed = 16f;

    void Start() {

    }

    void Update() {
        float travel = speed * Time.deltaTime;
        if (remainingDistance <= 0)
            Destroy(gameObject);
        if (remainingDistance - travel >= 0) {
            transform.Translate(0, 0, speed * Time.deltaTime);
        } else {
            transform.Translate(0, 0, remainingDistance);
        }
        remainingDistance -= travel;
    }

    public void setRemainingDistance(float remainingDistance) {
        this.remainingDistance = remainingDistance;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }
}
