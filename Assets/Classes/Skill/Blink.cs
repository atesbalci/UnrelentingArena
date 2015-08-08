using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private float timeBeforeDestruction;

    public Blink()
        : base() {
        timeBeforeDestruction = 1.5f;
        type = SkillType.Blink;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        gameObject.transform.position = targetPosition;
        player.gameObject.transform.position = targetPosition;
        player.gameObject.GetComponent<PlayerScript>().LeaveFadingImage();
    }

    public override void Update() {
        base.Update();
        timeBeforeDestruction -= Time.deltaTime;
        if (timeBeforeDestruction <= 0)
            dead = true;
    }
}
