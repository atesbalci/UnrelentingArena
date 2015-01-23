using UnityEngine;
using System.Collections;

public class TargetSkill : Skill {
    protected bool initialized;

    public TargetSkill() {
        initialized = false;
    }

    public override void update(GameObject gameObject) {
        if (!initialized) {
            if (Vector3.Distance(gameObject.transform.position, targetPosition) <= range) {
                gameObject.transform.position = new Vector3(targetPosition.x, gameObject.transform.position.y, targetPosition.z);
            } else {
                gameObject.transform.Translate(0, 0, range);
            }
            initialized = true;
        }
    }
}
