using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private float timeBeforeDestruction;
    private bool blinkDone;

    public Blink() {
        blinkDone = false;
        timeBeforeDestruction = 1.5f;
    }

    public override void Update(GameObject gameObject) {
        base.Update(gameObject);
        if (!blinkDone) {
            player.SchedulePositionChange(gameObject.transform.position);
            blinkDone = true;
        } else {
            timeBeforeDestruction -= Time.deltaTime;
            if (timeBeforeDestruction <= 0)
                Destroy(gameObject);
        }
    }
}
