using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {
    public GameObject shield;
    public LensFlare flare;

    public bool blocking { get; set; }

    private Player player;
    public NetworkView view;
    private PlayerMove playerMove;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        playerMove = GetComponent<PlayerMove>();
        blocking = false;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float bp = player.blockingPoints;
            float be = player.blockingExhaust;
            bool bl = blocking;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
            stream.Serialize(ref bl);
        } else {
            float bp = 0;
            float be = 0;
            bool bl = blocking;
            stream.Serialize(ref bp);
            stream.Serialize(ref be);
            stream.Serialize(ref bl);
            player.blockingPoints = bp;
            player.blockingExhaust = be;
            blocking = bl;
        }
    }

    void Update() {
        if (view.isMine) {
            if (blocking && (Input.GetKeyUp(GameManager.instance.keys[(int)GameBindings.Block]) || player.blockingPoints < 0)) {
                blocking = false;
                player.blockingExhaust = 1;
            } else if (Input.GetKeyDown(GameManager.instance.keys[(int)GameBindings.Block]) && player.canCast) {
                if (player.blockingPoints >= player.statSet.maxBlockingPoints) {
                    blocking = true;
                }
            }
        }
        if (player.blockingExhaust <= 0) {
            if (blocking)
                player.blockingPoints -= Time.deltaTime;
            else
                player.blockingPoints += Time.deltaTime * player.statSet.blockingRegen;
        } else {
            player.blockingExhaust -= Time.deltaTime;
        }
        if (player.blockingPoints >= player.statSet.maxBlockingPoints) {
            player.blockingPoints = player.statSet.maxBlockingPoints;
        }
        if (blocking) {
            shield.SetActive(true);
            flare.brightness = Mathf.Lerp(0.1f, 0.4f, player.blockingPoints / player.statSet.maxBlockingPoints);
            if (view.isMine) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    playerMove.destinationPosition = Vector3.Lerp(transform.position, ray.GetPoint(hitdist), 0.005f);
                }
            }
        } else
            shield.SetActive(false);
    }
}
