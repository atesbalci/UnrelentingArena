using UnityEngine;
using System.Collections;

public class Charge : Skill {
    private const float PERIOD = 0.1f;

    private float time;
    private bool charging;
    private bool damaging;
    private ParticleSystem effect;

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        effect = gameObject.GetComponentInChildren<ParticleSystem>();
        player.gameObject.transform.rotation = Quaternion.LookRotation(targetPosition - player.gameObject.transform.position);
        effect.gameObject.transform.rotation = player.gameObject.transform.rotation;
        gameObject.transform.SetParent(player.gameObject.transform);
        time = 0;
        charging = true;
        damaging = false;
        effect.startColor = player.color;
        effect.startRotation = player.gameObject.transform.eulerAngles.y * Mathf.Deg2Rad;
        effect.gameObject.SetActive(false);
    }

    public override void Update() {
        if (charging) {
            player.AddBuff(new ChargeBuff(player));
            time += Time.deltaTime;
            if (time / PERIOD >= 1) {
                time -= PERIOD;
                player.gameObject.GetComponent<PlayerScript>().LeaveFadingImage();
            }
            player.gameObject.transform.position = Vector3.MoveTowards(player.gameObject.transform.position, targetPosition, 20 * Time.deltaTime);
            if (player.gameObject.transform.position == targetPosition) {
                player.gameObject.GetComponent<PlayerMove>().destinationPosition = player.gameObject.transform.position;
                time = 0;
                gameObject.transform.SetParent(null);
                charging = false;
                damaging = true;
                effect.gameObject.SetActive(true);
                effect.Stop();
                time = effect.startLifetime;
            }
        } else {
            time -= Time.deltaTime;
            damaging = false;
            if (time <= 0)
                Network.Destroy(gameObject);
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (damaging) {
            collider.GetComponent<PlayerScript>().Knockback(gameObject.transform.rotation * Vector3.forward, 10, 30);
        }
    }
}
