using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {
    public float movementSpeed = 8;
    public KeyCode moveButton = KeyCode.Mouse1;

    private Transform myTransform;
    private Vector3 destinationPosition;
    private float destinationDistance;
    private float clickMoveSpeed = 0;
    private bool stopped = false;

    void Start() {
        myTransform = transform;
        destinationPosition = myTransform.position;
    }

    void Update() {
        if (!stopped) {
            destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

            if (destinationDistance < .5f) {
                clickMoveSpeed = 0;
                animation.Play("idle");
            } else if (destinationDistance > .5f) {
                clickMoveSpeed = movementSpeed;
                animation.Play("run");
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
        }
    }

    public void stop() {
        stopped = true;
        animation.Play("idle");
    }

    public void start() {
        stopped = false;
        resetMovement();
    }

    public void resetMovement() {
        destinationPosition = transform.position;
    }
}