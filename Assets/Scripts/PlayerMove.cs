using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private const float TURN_RATE = 360;

    public NetworkView view;

    public bool moving { get; set; }
    public Quaternion targetRotation { get; set; }

    private Player player;
    private Animator anim;
    private Plane playerPlane;
    private PlayerSkill playerSkill;

    void Start() {
        anim = GetComponent<Animator>();
        moving = true;
        player = GetComponent<PlayerScript>().player;
        playerPlane = new Plane(Vector3.up, transform.position);
        playerSkill = GetComponent<PlayerSkill>();
    }

    void Update() {
        if (!player.dead && !GameManager.instance.locked) {
            if (view.isMine) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                Quaternion cursorRotation = Quaternion.identity;
                if (playerPlane.Raycast(ray, out hitdist)) {
                    cursorRotation = Quaternion.LookRotation(ray.GetPoint(hitdist) - transform.position, Vector3.up);
                }
                if (!playerSkill.currentlyCasting) {
                    targetRotation = cursorRotation;
                } else {
                    float angle = Quaternion.Angle(transform.rotation, cursorRotation);
                    if (Mathf.Abs(angle) > 60) {
                        targetRotation = Quaternion.RotateTowards(transform.rotation, cursorRotation, angle - 60);
                    }
                }
                if (Input.GetKey(GameInput.instance.keys[(int)GameBinding.Move]) && player.currentSpeed > 0.01f)
                    moving = true;
                else
                    moving = false;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * TURN_RATE);
            if(moving)
                transform.Translate(0, 0, player.currentSpeed * Time.deltaTime);
            anim.SetFloat("Speed", moving ? player.currentSpeed : 0);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            bool moving = this.moving;
            Quaternion targetRotation = this.targetRotation;
            stream.Serialize(ref moving);
            stream.Serialize(ref targetRotation);
        } else {
            bool moving = false;
            Quaternion targetRotation = Quaternion.identity;
            stream.Serialize(ref moving);
            stream.Serialize(ref targetRotation);
            this.moving = moving;
            this.targetRotation = targetRotation;
        }
        GetComponentInChildren<PlayerStatusScript>().Update();
    }
}