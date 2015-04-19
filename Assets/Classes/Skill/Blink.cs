using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private float timeBeforeDestruction;

    public Blink()
        : base() {
        timeBeforeDestruction = 1.5f;
    }

    public override void Start(GameObject gameObject) {
        player.SchedulePositionChange(targetPosition);
        player.leaveImage = true;
    }

    public override void Update(GameObject gameObject) {
        base.Update(gameObject);
        timeBeforeDestruction -= Time.deltaTime;
        if (timeBeforeDestruction <= 0)
            Destroy(gameObject);
    }
}
