using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    private Vector3 cPos;

    public float ratio = 0.025f;
    public float speed = 1.5f;
    public int xLimit = 30;
    public int zLimit = 30;
    public int zoomLimit = 20;
    public float zoomSpeed = 1.5f;


    void Start() {
        cPos = new Vector3(0, 0, 0);
    }

    void Update() {
        if (GUIUtility.hotControl == 0) {
            Vector3 mPos = Input.mousePosition;
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            float x = 0;
            float y = 0;
            float z = 0;

            if (mPos.x >= Screen.width * (1 - ratio)) {
                x += speed;
            } else if (mPos.x <= Screen.width * ratio) {
                x -= speed;
            }

            if (mPos.y >= Screen.height * (1 - ratio)) {
                z += speed;
            } else if (mPos.y <= Screen.height * ratio) {
                z -= speed;
            }

            if (wheel < 0) {
                y += zoomSpeed;
            } else if (wheel > 0) {
                y -= zoomSpeed;
            }

            if (Mathf.Abs(cPos.x + x) > xLimit) {
                cPos.x = cPos.x > 0 ? xLimit : -xLimit;
                x = 0;
            }
            if (Mathf.Abs(cPos.z + z) > zLimit) {
                cPos.z = cPos.z > 0 ? zLimit : -zLimit;
                z = 0;
            }
            if ((cPos.y + y) < -zoomLimit || (cPos.y + y) > 0) {
                cPos.y = (cPos.y + y) < -zoomLimit ? -zoomLimit : 0;
                y = 0;
            }

            transform.Translate(0, 0, -y);
            transform.position += new Vector3(x, 0, z);
            cPos.x += x;
            cPos.z += z;
            cPos.y += y;
        }
    }
}