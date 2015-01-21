using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    public KeyCode moveButton = KeyCode.Mouse1;
    public Vector3 destinationPosition;

    private Transform myTransform;
    private float destinationDistance;
    private float clickMoveSpeed = 0;

    void Start() {
        myTransform = transform;
        destinationPosition = myTransform.position;
    }

    void Update() {
        Player player = GetComponent<PlayerScript>().player;
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if(clickMoveSpeed > 0.5f)
            animation.Play("run");
        else
            animation.Play("idle");
        if (player.currentSpeed > 0.5f) {
            if (destinationDistance < .5f) {
                clickMoveSpeed = 0;
            } else if (destinationDistance > .5f) {
                clickMoveSpeed = player.currentSpeed;
            }

            if (Input.GetKey(moveButton) && GUIUtility.hotControl == 0) {
                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist)) {
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destinationPosition = ray.GetPoint(hitdist);
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                    myTransform.rotation = targetRotation;
                }
            }
            if (destinationDistance > .5f) {
                myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, clickMoveSpeed * Time.deltaTime);
            }
        } else
            clickMoveSpeed = 0;

        if (player.movementReset) {
            destinationPosition = transform.position;
            player.movementReset = false;
        }
    }
}