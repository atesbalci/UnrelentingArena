using UnityEngine;
using System.Collections;

public class Knockback : Stun {
    public Vector3 direction { get; set; }
    public float remainingDistance { get; set; }
    public float speed { get; set; }

    protected GameObject playerObject;

    public Knockback(Player player, GameObject playerObject, Vector3 direction, float distance, float speed)
        : base(player) {
        duration = 3;
        this.direction = direction;
        remainingDistance = distance;
        this.speed = speed;
        this.playerObject = playerObject;
    }

    public override void update() {
        float travel = speed * Time.deltaTime;
        if (remainingDistance - travel <= 0) {
            travel = remainingDistance;
            player.removeBuff(this);
        }
        remainingDistance -= travel;
        playerObject.transform.position += direction * travel;
        base.update();
    }
}
