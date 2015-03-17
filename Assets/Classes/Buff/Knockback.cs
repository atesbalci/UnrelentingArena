using UnityEngine;
using System.Collections;

public class Knockback : Stun {
    public Vector3 direction { get; set; }
    public float remainingDistance { get; set; }
    public float speed { get; set; }

    private GameObject playerObject;
    private Animator animator;

    public Knockback(Player player, GameObject playerObject, Vector3 direction, float distance, float speed)
        : base(player) {
        duration = 3;
        this.direction = direction;
        remainingDistance = distance;
        this.speed = speed;
        this.playerObject = playerObject;
        animator = playerObject.GetComponent<Animator>();
    }

    public override void Update() {
        float travel = speed * Time.deltaTime;
        animator.SetBool("Knockback", true);
        if (remainingDistance - travel <= 0) {
            travel = remainingDistance;
            player.RemoveBuff(this);
        }
        remainingDistance -= travel;
        playerObject.transform.position += direction * travel;
        base.Update();
    }

    public override void Unbuff() {
        base.Unbuff();
        animator.SetBool("Knockback", false);
    }
}
