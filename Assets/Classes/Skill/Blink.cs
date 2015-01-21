using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private float timeBeforeDestruction;
    private bool blinkDone;

    public Blink(Player player)
        : base(player) {
        prefab = "Blink";
        blinkDone = false;
        timeBeforeDestruction = 1.5f;
    }

    public override void update(GameObject gameObject) {
        base.update(gameObject);
        if (!blinkDone) {
            player.schedulePositionChange(gameObject.transform.position);
            blinkDone = true;
        } else {
            timeBeforeDestruction -= Time.deltaTime;
            if (timeBeforeDestruction <= 0)
                MonoBehaviour.Destroy(gameObject);
        }
    }
}
