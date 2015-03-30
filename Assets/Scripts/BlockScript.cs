using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {
    void OnTriggerEnter(Collider collider) {
        SkillScript ss = collider.gameObject.GetComponent<SkillScript>();
        if (ss != null) {
            if (ss.skillType == SkillType.Fireball) {
                Network.Destroy(collider.gameObject);
            }
        }
    }
}
