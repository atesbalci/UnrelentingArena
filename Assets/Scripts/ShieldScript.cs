using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {
    public GameObject shield;
    public LensFlare flare;

    public bool blocking { get; set; }

    private Player player;
    private NetworkView view;
    private PlayerMove playerMove;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        playerMove = GetComponent<PlayerMove>();
        view = GetComponent<NetworkView>();
        if (player.owner == Network.player) {
            view.RPC("SwitchOwner", RPCMode.All, Network.AllocateViewID());
        }
        blocking = false;
    }

    [RPC]
    public void SwitchOwner(NetworkViewID newId) {
        view.viewID = newId;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float bp = player.blockingPoints;
            float be = player.blockingExhaust;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
        } else {
            float bp = 0;
            float be = 0;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
            player.blockingPoints = bp;
            player.blockingExhaust = be;
        }
    }

    void Update() {
        if (view.isMine) {
            if (blocking && (Input.GetKeyUp(GameManager.instance.keys[(int)GameBindings.Block]) || player.blockingPoints < 0)) {
                blocking = false;
                player.blockingExhaust = 1;
            } else if (Input.GetKeyDown(GameManager.instance.keys[(int)GameBindings.Block]) && player.canCast) {
                if (player.blockingPoints >= player.maxBlockingPoints) {
                    blocking = true;
                }
            }
        }
        if (player.blockingExhaust <= 0) {
            if (blocking)
                player.blockingPoints -= Time.deltaTime;
            else
                player.blockingPoints += Time.deltaTime * player.blockingRegen;
        } else {
            player.blockingExhaust -= Time.deltaTime;
        }
        if (player.blockingPoints >= player.maxBlockingPoints) {
            player.blockingPoints = player.maxBlockingPoints;
        }
        if (blocking) {
            shield.SetActive(true);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist)) {
                playerMove.Move(transform.position, Quaternion.LookRotation(ray.GetPoint(hitdist) - transform.position));
            }
            flare.brightness = Mathf.Lerp(0.4f, 0.1f, player.blockingPoints / player.maxBlockingPoints);
        } else
            shield.SetActive(false);
    }
}
