using UnityEngine;
using System.Collections;

public abstract class Skill {
    private float _range;
    public float range { get { return _range; } set { _range = value; remainingDistance = value; } }
    public float speed { get; set; }
    public string prefab { get; set; }

    protected float remainingDistance;

    public Skill() {
        range = 10;
        speed = 16;
        prefab = "";
    }

    public void update(GameObject gameObject, float delta) {
        float travel = speed * delta;
        if (remainingDistance <= 0)
            MonoBehaviour.Destroy(gameObject);
        if (remainingDistance - travel >= 0) {
            gameObject.transform.Translate(0, 0, speed * delta);
        } else {
            gameObject.transform.Translate(0, 0, remainingDistance);
        }
        remainingDistance -= travel;
    }

    public void collisionWithSkill(GameObject gameObject, Collision collision, Skill skill) {
        MonoBehaviour.Destroy(gameObject);
        MonoBehaviour.Destroy(collision.gameObject);
    }

    public void collisionWithOtherObject(GameObject gameObject, Collision collision) {

    }
}
