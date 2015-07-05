using UnityEngine;
using System.Collections;

public class AnimationSynchronizer : MonoBehaviour {
    public NetworkView view;

    private Animator anim;

    private bool casting, knockback;
    private float speed;

	void Start () {
        anim = GetComponent<Animator>();
        if (view.isMine) {
            casting = anim.GetBool("Casting");
            knockback = anim.GetBool("Knockback");
            speed = anim.speed;
        }
	}
	
	void Update () {
        if (view.isMine) {
            bool castingNew = anim.GetBool("Casting");
            bool knockbackNew = anim.GetBool("Knockback");
            float speedNew = anim.speed;

            if (casting != castingNew) {
                view.RPC("SyncCasting", RPCMode.Others, castingNew);
                casting = castingNew;
            }
            if (knockback != knockbackNew) {
                view.RPC("SyncKnockback", RPCMode.Others, knockbackNew);
                knockback = knockbackNew;
            }
            if (speed != speedNew) {
                view.RPC("SyncSpeed", RPCMode.Others, speedNew);
                speed = speedNew;
            }
        }
	}

    [RPC]
    private void SyncCasting(bool b) {
        anim.SetBool("Casting", b);
    }

    [RPC]
    private void SyncKnockback(bool b) {
        anim.SetBool("Knockback", b);
    }

    [RPC]
    private void SyncSpeed(float f) {
        anim.speed = f;
    }
}
