using UnityEngine;
using System.Collections;

public class Orb : Skill {
    private enum OrbState {
        Rising, Falling, Landed, Active, Ending
    }

    private ParticleSystem area;
    private Renderer renderer;
    private OrbState state;
    private Vector3 verticalTarget;
    float time;

    public Orb() {
        state = OrbState.Rising;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        area = gameObject.GetComponentsInChildren<ParticleSystem>()[0];
        renderer = gameObject.GetComponent<Renderer>();
        area.startColor = player.color;
        area.gameObject.SetActive(false);
        verticalTarget = new Vector3(gameObject.transform.position.x, 10, gameObject.transform.position.z);
    }

    public override void Update() {
        if (state == OrbState.Rising) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, verticalTarget, Time.deltaTime * 2);
            if (verticalTarget.y - gameObject.transform.position.y < 0.5f) {
                state = OrbState.Falling;
            }
        } else if (state == OrbState.Falling) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, Time.deltaTime * 4);
            if (Mathf.Abs(targetPosition.y - gameObject.transform.position.y) < 0.1f) {
                time = 1;
                gameObject.transform.position = targetPosition;
                state = OrbState.Landed;
            }
        } else if (state == OrbState.Landed) {
            time -= Time.deltaTime;
            if (time <= 0) {
                state = OrbState.Active;
            }
        } else if (state == OrbState.Active) {
            area.gameObject.SetActive(true);
            time = area.startLifetime;
            state = OrbState.Ending;
            area.Stop();
        } else {
            time -= Time.deltaTime;
            renderer.material.color = new Color(player.color.r, player.color.g, player.color.b, Mathf.Lerp(1, 0, time / area.startLifetime));
            if (time <= 0) {
                Network.Destroy(gameObject);
            }
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (state == OrbState.Active) {
            player.Damage(damage, this.player);
            collider.gameObject.GetComponent<PlayerScript>().Knockback(gameObject.transform.position - collider.gameObject.transform.position, 10, 30);
        }
    }
}
