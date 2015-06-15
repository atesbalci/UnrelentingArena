using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public Vector3 destinationPosition { get; set; }
    public NetworkView view;
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
            if (view.isMine && Input.GetKey(GameInput.instance.keys[(int)GameBinding.Move])) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    Move(ray.GetPoint(hitdist));
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