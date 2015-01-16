using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {
	public float ratio = 0.025f;
	public float speed = 1.5f;
	public int xLimit = 30;
	public int zLimit = 30;
	public int zoomLimit = 10;
	public float zoomSpeed = 1.5f;

	
	void Start () {
	
	}

	void Update () {
		Vector3 mPos = Input.mousePosition;
		Vector3 cPos = transform.position;
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

		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			y += zoomSpeed;
		} else if(Input.GetAxis ("Mouse ScrollWheel") > 0) {
			y -= zoomSpeed;
		}

		if (Mathf.Abs (cPos.x + x) > xLimit) {
			transform.position = new Vector3(cPos.x > 0 ? xLimit : -xLimit, cPos.y, cPos.z);
			x = 0;
		}
		if (Mathf.Abs (cPos.z + z) > zLimit) {
			transform.position = new Vector3(cPos.x, cPos.y, cPos.z > 0 ? zLimit : -zLimit);
			z = 0;
		}
		if ((cPos.y + y) < -zoomLimit || (cPos.y + y) > 0) {
			transform.position = new Vector3(cPos.x, (cPos.y + y) < -zoomLimit ? -zoomLimit : 0, cPos.z);
			y = 0;
		}

		transform.Translate (new Vector3(x, y, z));
	}
}
