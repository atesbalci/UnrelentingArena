using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BlockScript : NetworkBehaviour {
    private Player player;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
    }

    void OnTriggerEnter(Collider collider) {
        if (Network.isServer) {
            SkillScript ss = collider.gameObject.GetComponent<SkillScript>();
            if (ss != null) {
                if (ss.skill is SkillShot) {
                    Quaternion newRotation = Quaternion.Inverse(collider.transform.rotation);
                    float angle = Quaternion.Angle(transform.rotation, newRotation);
                    newRotation = Quaternion.AngleAxis(angle * 2, newRotation * Vector3.forward);
                    RpcBlockEvent(newRotation);
                }
            }
        }
    }

    [ClientRpc]
    public void RpcBlockEvent(Quaternion rotation) {
        //SkillShot ss = obj.GetComponent<SkillScript>().skill as SkillShot;
        //ss.remainingDistance = ss.range;
        //obj.transform.rotation = rotation;
        //ss.player = GameManager.instance.playerList[player.owner].currentPlayer;
    }
}
