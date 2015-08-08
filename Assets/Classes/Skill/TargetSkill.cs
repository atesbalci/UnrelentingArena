using UnityEngine;
using System.Collections;

public class TargetSkill : Skill {
    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        targetPosition = Vector3.MoveTowards(gameObject.transform.position, targetPosition, preset.range);
    }
}
