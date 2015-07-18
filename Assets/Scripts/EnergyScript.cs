using UnityEngine;
using System.Collections;

public class EnergyScript : MonoBehaviour {
    public NetworkView view;
    public GameObject pulse;

    public float dodging { get; set; }

    private Player player;
    private PlayerMove playerMove;
    private Animator anim;
    private Roll buff;
    private Material[] trail;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        playerMove = GetComponent<PlayerMove>();
        dodging = 0;
        anim = GetComponent<Animator>();
        buff = new Roll(player);
        trail = GetComponent<PlayerScript>().trail.materials;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float bp = player.energyPoints;
            float be = player.energyExhaust;
            float bl = dodging;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
            stream.Serialize(ref bl);
        } else {
            float bp = 0;
            float be = 0;
            float bl = dodging;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
            stream.Serialize(ref bl);
            player.energyPoints = bp;
            player.energyExhaust = be;
            dodging = bl;
        }
    }

    void Update() {
        if (view.isMine) {
            if (dodging > 0) {
                player.energyExhaust = 1;
                anim.speed = 1;
                dodging -= Time.deltaTime;
                if (dodging <= 0) {
                    dodging = 0;
                }
                buff.speed = Mathf.Lerp(0, 15, dodging / 0.7f - 0.2f);
                player.AddBuff(buff);
                playerMove.destinationPosition = transform.position + transform.rotation * Vector3.forward * 10;
            }
            if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Dodge]) && player.canCast && player.energyPoints >= 0.5f) {
                dodging = 0.7f;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdst = 0;
                (new Plane(Vector3.up, transform.position)).Raycast(ray, out hitdst);
                transform.rotation = Quaternion.LookRotation(ray.GetPoint(hitdst) - transform.position, Vector3.up);
                player.energyPoints -= 0.5f;
                player.energyExhaust = 1;
            } else if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Pulse]) && player.canCast && player.energyPoints >= 0.5f) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdst = 0;
                (new Plane(Vector3.up, transform.position)).Raycast(ray, out hitdst);
                Network.Instantiate(pulse, transform.position, Quaternion.LookRotation(ray.GetPoint(hitdst) - transform.position), 0);
                player.energyPoints -= 0.5f;
                player.energyExhaust = 1;
            }
        }
        if (player.energyExhaust <= 0) {
            player.energyPoints += Time.deltaTime * player.statSet.energyRegen;
        } else {
            player.energyExhaust -= Time.deltaTime;
        }
        if (player.energyPoints >= player.statSet.maxEnergyPoints) {
            player.energyPoints = player.statSet.maxEnergyPoints;
        }
        anim.SetBool("Dodging", dodging > 0);
        bool colorDependent = false;
        foreach (Material m in trail) {
            m.SetColor("_TintColor", dodging > 0 ? (!colorDependent ? player.color : Color.white) : Color.Lerp(m.GetColor("_TintColor"), Color.clear, 0.1f));
            colorDependent = true;
        }
    }
}
