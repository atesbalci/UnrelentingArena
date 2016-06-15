using UnityEngine;
using System.Collections;

public class EnergyScript : MonoBehaviour {
    public NetworkView view;
    public GameObject pulse;

    public float dodging { get; set; }

    private Player player;
    private PlayerMove playerMove;
    private Animator anim;
    private Roll rollBuff;
    private Swing swingBuff;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        playerMove = GetComponent<PlayerMove>();
        dodging = 0;
        anim = GetComponent<Animator>();
        rollBuff = new Roll(player);
        swingBuff = new Swing(player);
        player.AddBuff(rollBuff);
        player.AddBuff(swingBuff);
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
        if (view.isMine && !player.dead && !GameManager.instance.locked) {
            bool isSwinging = anim.GetCurrentAnimatorStateInfo(1).IsName("Hit1") ||
                anim.GetCurrentAnimatorStateInfo(1).IsName("Hit2") ||
                anim.GetCurrentAnimatorStateInfo(1).IsName("Hit3");
            if (!isSwinging)
                swingBuff.active = false;
            else
                swingBuff.active = true;
            if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Dodge]) && player.canCast && player.energyPoints >= 0.5f) {
                dodging = 0.7f;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdst = 0;
                (new Plane(Vector3.up, transform.position)).Raycast(ray, out hitdst);
                transform.rotation = Quaternion.LookRotation(ray.GetPoint(hitdst) - transform.position, Vector3.up);
                player.energyPoints -= 0.5f;
                player.energyExhaust = 1;
                if (player.modifier == ComboModifier.Momentum) {
                    dodging *= 1.25f;
                    player.modifier = ComboModifier.Composure;
                } else
                    player.modifier = ComboModifier.Momentum;
            } else if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Pulse]) && (player.canCast || (swingBuff.active && anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)) && player.energyPoints >= 0.2f) {
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //float hitdst = 0;
                //(new Plane(Vector3.up, transform.position)).Raycast(ray, out hitdst);
                //Network.Instantiate(pulse, transform.position, Quaternion.LookRotation(ray.GetPoint(hitdst) - transform.position), 0);
                //player.modifier = ComboModifier.Fury;
                view.RPC("Swing", RPCMode.AllBuffered);
            }
        }
        if (dodging > 0) {
            player.energyExhaust = 1;
            anim.speed = 1;
            dodging -= Time.deltaTime;
            if (dodging <= 0) {
                dodging = 0;
            }
            rollBuff.speed = Mathf.Lerp(0, 15, dodging / 0.7f - 0.2f);
            playerMove.destinationPosition = transform.position + transform.rotation * Vector3.forward * 10;
            rollBuff.active = true;
        } else
            rollBuff.active = false;
        if (player.energyExhaust <= 0) {
            player.energyPoints += Time.deltaTime * player.statSet.energyRegen;
        } else {
            player.energyExhaust -= Time.deltaTime;
        }
        if (player.energyPoints >= player.statSet.maxEnergyPoints) {
            player.energyPoints = player.statSet.maxEnergyPoints;
        }
        anim.SetBool("Dodging", dodging > 0);
    }

    [RPC]
    public void Swing() {
        if (Network.isServer && anim.GetCurrentAnimatorStateInfo(1).IsName("Hit2"))
            view.RPC("Fury", RPCMode.All);
        anim.SetTrigger("Swing");
        player.energyPoints -= 0.2f;
        player.energyExhaust = 1;
    }

    [RPC]
    public void Fury() {
        player.modifier = ComboModifier.Fury;
    }
}
