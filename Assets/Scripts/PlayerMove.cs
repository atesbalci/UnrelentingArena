using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMove : NetworkBehaviour {
    [SyncVar]
    public Vector3 destinationPosition;

    private float moveSpeed = 0;
    private Player player;
    private Animator anim;
    private Plane playerPlane;

    void Start() {
        anim = GetComponent<Animator>();
        destinationPosition = transform.position;
        player = GetComponent<PlayerScript>().player;
        playerPlane = new Plane(Vector3.up, transform.position);
    }

    void Update() {
        if (!player.dead) {
            if (isLocalPlayer && Input.GetKey(GameInput.instance.keys[(int)GameBinding.Move])) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    CmdMove(ray.GetPoint(hitdist));
                }
            }
            anim.SetFloat("Speed", moveSpeed);
            float destinationDistance = Vector3.Distance(destinationPosition, transform.position);
            if (player.currentSpeed > 0.5f) {
                if (destinationDistance < .5f) {
                    moveSpeed = 0;
                } else if (destinationDistance > 0) {
                    moveSpeed = player.currentSpeed;
                }
                if (destinationDistance > .5f) {
                    transform.position = Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * moveSpeed);
                }
                if (destinationPosition - transform.position != Vector3.zero)
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(destinationPosition - transform.position), Time.deltaTime * 10);
            } else {
                moveSpeed = 0;
                if (destinationDistance > 1)
                    destinationPosition = transform.position;
            }
        }
    }

    [Command]
    public void CmdMove(Vector3 destination) {
        destinationPosition = destination;
        GetComponentInChildren<PlayerStatusScript>().Update();
    }
}