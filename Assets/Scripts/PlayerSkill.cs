using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    private ParticleSystem particles;
    private Player player;
    private Vector3 targetPoint;
    private int casting;
    private Animator anim;
    public NetworkView view;
    private PlayerMove playerMove;

    void Start() {
        particles = GetComponent<PlayerScript>().particles;
        player = GetComponent<PlayerScript>().player;
        casting = -1;
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        particles.Stop();
    }

    void Update() {
        if (!player.dead) {
            casting = -1;
            if (view.isMine && player.canCast) {
                for (int i = 0; i < 4; i++) {
                    if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Skill1 + i])) {
                        casting = i;
                        break;
                    }
                }
            }

            if (player.toBeCast != null) {
                //networkView.RPC("InstantiateSkill", RPCMode.All, player.toBeCast.skill.prefab, player.toBeCast.position,
                //    player.toBeCast.rotation, player.toBeCast.skill.level, player.toBeCast.targetPosition);
                InstantiateSkill(player.toBeCast.skill.skill, player.toBeCast.position,
                    player.toBeCast.rotation, player.toBeCast.targetPosition);
                player.toBeCast = null;
            } else if (player.canCast && casting > -1) {
                SkillPreset skill = null;
                skill = player.skillSet.TryToCast(casting);
                if (skill == null) {
                    casting = -1;
                    return;
                }
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist) && view.isMine)
                    targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                playerMove.destinationPosition = Vector3.MoveTowards(transform.position, targetPoint, 0.1f);
                player.AddBuff(new CastChannel(player, skill, new Vector3(transform.position.x, 1, transform.position.z), targetRotation, targetPoint));
                view.RPC("Cast", RPCMode.All);
            }
        }
    }

    [RPC]
    public void Cast() {
        particles.Play();
        anim.SetTrigger("Casting");
    }

    public void InstantiateSkill(SkillType skill, Vector3 position, Quaternion rotation, Vector3 targetPosition) {
        GameObject skillObject = Network.Instantiate(GameManager.instance.skills[(int)skill], position, rotation, 0) as GameObject;
        view.RPC("InitializeSkill", RPCMode.All, skillObject.GetComponent<NetworkView>().viewID, targetPosition);
    }

    [RPC]
    public void InitializeSkill(NetworkViewID id, Vector3 targetPosition) {
        GameObject skillObject = NetworkView.Find(id).gameObject;
        SkillScript skillScript = skillObject.GetComponent<SkillScript>();
        skillScript.Initialize();
        Skill skill = skillScript.skill;
        if (skill != null) {
            skill.targetPosition = targetPosition;
            skill.player = player;
        }
    }
}