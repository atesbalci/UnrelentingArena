using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private Vector3 destinationPosition;
    private float destinationDistance;
    private float clickMoveSpeed = 0;
    private ControlScript controlScript;
    private Player player;

    void Start() {
        destinationPosition = transform.position;
        controlScript = GetComponent<ControlScript>();
        player = GetComponent<PlayerScript>().player;
    }

    void Update() {
        destinationDistance = Vector3.Distance(destinationPosition, transform.position);

        if (clickMoveSpeed > 0.5f)
            animation.Play("run");
        else
            animation.Play("idle");
        if (player.currentSpeed > 0.5f) {
            if (destinationDistance < .5f) {
                clickMoveSpeed = 0;
            } else if (destinationDistance > .5f) {
                clickMoveSpeed = player.currentSpeed;
            }

            if (controlScript.move && GUIUtility.hotControl == 0) {
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist)) {
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destinationPosition = ray.GetPoint(hitdist);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 1);
                }
            }
            if (destinationDistance > .5f) {
                movePlayer(Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * clickMoveSpeed));
            }
        } else
            clickMoveSpeed = 0;

        if (player.movementReset) {
            destinationPosition = transform.position;
            player.movementReset = false;
        }
    }

    public void movePlayer(Vector3 target) {
        transform.position = Vector3.Lerp(transform.position, target, 1);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        Vector3 dest = new Vector3(0, 0, 0);
        Quaternion rot = new Quaternion(0, 0, 0, 0);
        if (stream.isWriting) {
            rot = transform.rotation;
            dest = destinationPosition;
            stream.Serialize(ref dest);
            stream.Serialize(ref rot);
        } else {
            stream.Serialize(ref dest);
            stream.Serialize(ref rot);
            destinationPosition = dest;
            transform.rotation = rot;
        }
    }
}