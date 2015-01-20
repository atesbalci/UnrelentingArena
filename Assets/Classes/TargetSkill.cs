using UnityEngine;
using System.Collections;

public class TargetSkill : Skill {
    protected bool initialized;

    public TargetSkill(Player player)
        : base(player) {
        initialized = false;
    }

    public override void update(GameObject gameObject, float delta, Vector3 targetPosition) {
        if (!initialized) {
            if (Vector3.Distance(gameObject.transform.position, targetPosition) <= range) {
                gameObject.transform.position = targetPosition;
            } else {
                gameObject.transform.Translate(0, 0, range);
            }
            initialized = true;
        }
    }
}
