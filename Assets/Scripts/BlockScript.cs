using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {
    public NetworkView view;

    void OnTriggerEnter(Collider collider) {
        if (Network.isServer) {
            SkillScript ss = collider.gameObject.GetComponent<SkillScript>();
            if (ss != null) {
                if (ss.skill is SkillShot) {
                    Quaternion newRotation = Quaternion.Inverse(collider.transform.rotation);
                    float angle = Quaternion.Angle(transform.rotation, newRotation);
                    newRotation = Quaternion.AngleAxis(angle * 2, newRotation * Vector3.forward);
                    view.RPC("BlockEvent", RPCMode.All, ss.view.viewID, newRotation);
                }
            }
        }
    }

    [RPC]
    public void BlockEvent(NetworkViewID id, Quaternion rotation) {
        GameObject obj = NetworkView.Find(id).gameObject;
        SkillShot ss = obj.GetComponent<SkillScript>().skill as SkillShot;
        ss.remainingDistance = ss.range;
        obj.transform.rotation = rotation;
    }
}
