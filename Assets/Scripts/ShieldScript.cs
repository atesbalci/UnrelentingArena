using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ShieldScript : NetworkBehaviour {
    public GameObject shield;
    public LensFlare flare;

    [SyncVar]
    public bool blocking;

    [SyncVar]
    private float blockingPoints;
    [SyncVar]
    private float blockingExhaust;

    private Player player;
    private PlayerMove playerMove;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        playerMove = GetComponent<PlayerMove>();
        blocking = false;
    }

    [Command]
    public void CmdSync() {
        blockingPoints = player.blockingPoints;
        blockingExhaust = player.blockingExhaust;
    }

    void Update() {
        player.blockingExhaust = blockingExhaust;
        player.blockingPoints = blockingPoints;
        if (isLocalPlayer) {
            if (blocking && (Input.GetKeyUp(GameInput.instance.keys[(int)GameBinding.Block]) || player.blockingPoints < 0)) {
                blocking = false;
                player.blockingExhaust = 1;
            } else if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Block]) && player.canCast) {
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
            if (isLocalPlayer) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    playerMove.destinationPosition = Vector3.Lerp(transform.position, ray.GetPoint(hitdist), 0.05f);
                }
            }
        } else
            shield.SetActive(false);
    }
}
