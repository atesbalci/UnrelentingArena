using UnityEngine;
using System.Collections;

public class MovementStabilizer : MonoBehaviour {
    public NetworkView view;

    private Vector3 position;
    private Quaternion rotation;

    public void FixPosition() {
        transform.position = Vector3.MoveTowards(new Vector3(0, transform.position.z, 0), transform.position, GameManager.PLATFORM_RADIUS);
    }

    void Update() {
        FixPosition();
        if (!view.isMine) {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        FixPosition();
        if (stream.isWriting) {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        } else {
            Vector3 pos = new Vector3();
            Quaternion rot = new Quaternion();
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            position = pos;
            rotation = rot;
        }
    }
}
