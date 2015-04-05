using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public Vector3 destinationPosition { get; set; }
    public NetworkView view;
    private float moveSpeed = 0;
    private ControlScript controlScript;
    private Player player;
    private Animator anim;
    private Plane playerPlane;

    void Start() {
        anim = GetComponent<Animator>();
        destinationPosition = transform.position;
        controlScript = GetComponent<ControlScript>();
        player = GetComponent<PlayerScript>().player;
        playerPlane = new Plane(Vector3.up, transform.position);
    }

    void Update() {
        if (!player.dead) {
            anim.SetFloat("Speed", moveSpeed);
            float destinationDistance = Vector3.Distance(destinationPosition, transform.position);
            if (player.currentSpeed > 0.5f) {
                if (destinationDistance < .5f) {
                    moveSpeed = 0;
                } else if (destinationDistance > .5f) {
                    moveSpeed = player.currentSpeed;
                }

                if (view.isMine && Input.GetKey(GameManager.instance.keys[(int)GameBindings.Move])) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist)) {
                        //networkView.RPC("Move", RPCMode.All, ray.GetPoint(hitdist), Quaternion.LookRotation(ray.GetPoint(hitdist) - transform.position));
                        Move(ray.GetPoint(hitdist));
                    }
                }
                if (destinationDistance > .5f) {
                    MovePlayer(Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * moveSpeed));
                }
                transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(destinationPosition - transform.position), Time.deltaTime * 10);
            } else {
                moveSpeed = 0;
                if (destinationDistance > 1)
                    destinationPosition = transform.position;
            }
        }
    }

    public void MovePlayer(Vector3 target) {
        transform.position = Vector3.Lerp(transform.position, target, 1);
    }

    [RPC]
    public void Move(Vector3 destination) {
        destinationPosition = destination;
        GetComponentInChildren<PlayerStatusScript>().Update();
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            Vector3 destination = destinationPosition;
            stream.Serialize(ref destination);
        } else {
            Vector3 destination = new Vector3();
            stream.Serialize(ref destination);
            destinationPosition = destination;
        }
        GetComponentInChildren<PlayerStatusScript>().Update();
    }
}