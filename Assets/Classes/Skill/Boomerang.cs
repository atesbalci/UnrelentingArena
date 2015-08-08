using UnityEngine;
using System.Collections;

public class Boomerang : SkillShot {
    private bool _returning;
    private bool returning { get { return _returning; } set { _returning = value; if (returning) player.RemoveBuff(buff); } }
    private GameObject model;
    private Stun buff;
    private Plane plane;

    public Boomerang()
        : base() {
        type = SkillType.Boomerang;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        speed = 5;
        model = gameObject.GetComponentsInChildren<Transform>()[1].gameObject;
        returning = false;
        gameObject.GetComponentsInChildren<MeshRenderer>()[1].material.SetColor("_EmissionColor", player.color * Mathf.LinearToGammaSpace(4f));
        buff = new Stun(player, 100);
        player.AddBuff(buff);
        plane = new Plane(Vector3.up, gameObject.transform.position);
    }

    public override void Update() {
        model.transform.rotation = Quaternion.Euler(model.transform.rotation.eulerAngles.x, model.transform.rotation.eulerAngles.y + Time.deltaTime * 720, model.transform.rotation.eulerAngles.z);
        if (!returning) {
            base.Update();
            if (GameManager.instance.playerData.currentPlayer == player) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                Vector3 pos = Vector3.zero;
                if (plane.Raycast(ray, out hitdist)) {
                    pos = ray.GetPoint(hitdist);
                }
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                    Quaternion.LookRotation(pos - player.gameObject.transform.position), Time.deltaTime * 10);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,
                    Vector3.MoveTowards(player.gameObject.transform.position, pos, preset.range - remainingDistance), Time.deltaTime * 10);
            }
            if (maxRange)
                returning = true;
        } else {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.gameObject.transform.position, Time.deltaTime * speed * 2);
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (!returning) {
            player.Damage(preset.damage, this.player);
            Vector3 direction = gameObject.transform.rotation * Vector3.forward;
            collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, preset.knockbackDistance, preset.knockbackSpeed);
            returning = true;
        }
    }

    public override void CollisionWithSelf(Collider collider) {
        if (returning) {
            dead = true;
            player.RemoveBuff(buff);
        }
    }
}
