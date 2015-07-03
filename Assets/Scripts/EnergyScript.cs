using UnityEngine;
using System.Collections;

public class EnergyScript : MonoBehaviour {
    public GameObject shield;
    public LensFlare flare;
    public GameObject pulse;

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
            float bp = player.energyPoints;
            float be = player.energyExhaust;
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
            player.energyPoints = bp;
            player.energyExhaust = be;
            blocking = bl;
        }
    }

    void Update() {
        if (view.isMine) {
            if (blocking && (Input.GetKeyUp(GameInput.instance.keys[(int)GameBinding.Block]) || player.energyPoints < 0)) {
                blocking = false;
                player.energyExhaust = 1;
            } else if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Block]) && player.canCast) {
                if (player.energyPoints >= player.statSet.maxEnergyPoints) {
                    blocking = true;
                }
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
            if (blocking)
                player.energyPoints -= Time.deltaTime;
            else
                player.energyPoints += Time.deltaTime * player.statSet.energyRegen;
        } else {
            player.energyExhaust -= Time.deltaTime;
        }
        if (player.energyPoints >= player.statSet.maxEnergyPoints) {
            player.energyPoints = player.statSet.maxEnergyPoints;
        }
        if (blocking) {
            shield.SetActive(true);
            flare.brightness = Mathf.Lerp(0.1f, 0.4f, player.energyPoints / player.statSet.maxEnergyPoints);
            if (view.isMine) {
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
