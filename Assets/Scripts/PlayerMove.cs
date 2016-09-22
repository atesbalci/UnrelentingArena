using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public Vector3 destinationPosition { get; set; }
    public NetworkView view;

    private float moveSpeed = 0;
    private Player player;
    private Animator anim;
    private Plane playerPlane;
    private bool moving;

    void Start() {
        anim = GetComponent<Animator>();
        destinationPosition = transform.position;
        player = GetComponent<PlayerScript>().player;
        playerPlane = new Plane(Vector3.up, transform.position);
    }

    void Update() {
        if (!player.dead && !GameManager.instance.locked) {
            if (view.isMine && Input.GetKey(GameInput.instance.keys[(int)GameBinding.Move])) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    Move(ray.GetPoint(hitdist));
                }
            }
            float destinationDistance = Vector3.Distance(destinationPosition, transform.position);
            if (player.currentSpeed > 0.5f) {
                anim.speed = player.currentSpeed / player.statSet.movementSpeed;
                if (destinationDistance < .5f) {
                    moveSpeed = 0;
                } else if (destinationDistance > 0) {
                    moveSpeed = player.currentSpeed;
                }
                if (destinationDistance > .5f) {
                    transform.position = Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * moveSpeed);
                }
            } else {
                moveSpeed = 0;
                if (destinationDistance > 1)
                    destinationPosition = transform.position;
            }
            if (destinationPosition - transform.position != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(destinationPosition - transform.position), Time.deltaTime * 10);
            anim.SetFloat("Speed", moveSpeed);
        }
    }

    public void Move(Vector3 destination) {
		destination = new Vector3(destination.x, transform.position.y, destination.z);
        GetComponentInChildren<PlayerStatusScript>().Update();
        destinationPosition = Vector3.MoveTowards(new Vector3(0, transform.position.y, 0), destination, GameManager.PLATFORM_RADIUS);
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