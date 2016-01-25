using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private SpriteRenderer sprite;

    public Blink()
        : base() {
        type = SkillType.Blink;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        player.gameObject.GetComponent<PlayerScript>().LeaveFadingImage();
        player.gameObject.transform.position = targetPosition;
        player.gameObject.GetComponent<PlayerMove>().destinationPosition = targetPosition;
        gameObject.GetComponentInChildren<TrailRenderer>().material.SetColor("_TintColor", player.color);
        sprite.color = player.color;
    }

    public override void Update() {
        base.Update();
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, Time.deltaTime * 15);
        if (Mathf.Abs(Vector3.Distance(gameObject.transform.position, targetPosition)) <= 0.1f)
            dead = true;
    }

    public override void UpdateEnd() {
        sprite.color = Color.Lerp(sprite.color, Color.clear, Time.deltaTime * 10);
        if (sprite.color.a < 0.05f)
            base.UpdateEnd();
    }
}
