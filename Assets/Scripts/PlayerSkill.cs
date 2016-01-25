﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    private ParticleSystem particles;
    private Player player;
    private Vector3 targetPoint;
    private int casting;
    private Animator anim;
    public NetworkView view;
    private PlayerMove playerMove;
    public GameObject chest;
    public Image range;
    private bool currentlyCasting;

    void Start() {
        particles = GetComponent<PlayerScript>().particles;
        player = GetComponent<PlayerScript>().player;
        casting = -1;
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        particles.Stop();
        range.color = new Color(player.color.r, player.color.g, player.color.b, range.color.a);
        currentlyCasting = false;
    }

    void Update() {
        if (!player.dead) {
            casting = -1;
            if (view.isMine && player.canCast && !GameManager.instance.locked) {
                for (int i = 0; i < 4; i++) {
                    if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Skill1 + i])) {
                        casting = i;
                        break;
                    }
                }
            }

            if (player.canCast && casting > -1 && !currentlyCasting) {
                SkillPreset skill = null;
                skill = player.skillSet.TryToCast(casting);
                if (skill != null) {
                    player.casting = true;
                    StartCoroutine("CastSkill", skill);
                } else
                    casting = -1;
            }

            if (!player.casting)
                CancelCast();
        }
        range.transform.eulerAngles = new Vector3(range.transform.eulerAngles.x, transform.eulerAngles.y, range.transform.eulerAngles.z);

        //Vector3 forward = transform.rotation * Vector3.forward;
        //Vector3 left = transform.rotation * Vector3.left;
        //Vector3 right = transform.rotation * Vector3.right;
        //Debug.DrawLine(transform.position, 100 * (Quaternion.RotateTowards(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(left, Vector3.up), 60) * Vector3.forward), Color.red, 0, false);
        //Debug.DrawLine(transform.position, 100 * (Quaternion.RotateTowards(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(right, Vector3.up), 60) * Vector3.forward), Color.red, 0, false);
    }

    IEnumerator CastSkill(SkillPreset skill) {
        view.RPC("StartCast", RPCMode.All);
        ComboModifier modifier = player.modifier;
        player.modifier = ComboModifier.Composure;
        currentlyCasting = true;
        yield return new WaitForSeconds(0.5f);
        view.RPC("Cast", RPCMode.All);
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist = 0.0f;
        if (playerPlane.Raycast(ray, out hitdist) && view.isMine)
            targetPoint = ray.GetPoint(hitdist);
        Vector3 forward = transform.rotation * Vector3.forward;
        Vector3 castDir = (targetPoint - transform.position);
        castDir = Vector3.Normalize(new Vector3(castDir.x, 0, castDir.z));
        float castDist = Vector3.Distance(transform.position, targetPoint);
        targetPoint = transform.position + Quaternion.RotateTowards(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(castDir, Vector3.up), 60) * Vector3.forward * castDist;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        //playerMove.destinationPosition = Vector3.MoveTowards(transform.position, targetPoint, 0.1f);
        InstantiateSkill(skill.skill, new Vector3(transform.position.x, 1, transform.position.z),
            targetRotation, targetPoint, modifier);
        currentlyCasting = false;
        yield return null;
    }

    void LateUpdate() {
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Casting")) {
            Quaternion prev = chest.transform.rotation;
            chest.transform.LookAt(targetPoint);
            chest.transform.rotation = Quaternion.RotateTowards(prev, chest.transform.rotation, 60);
        }
    }

    public void CancelCast() {
        StopCoroutine("CastSkill");
    }

    [RPC]
    public void StartCast() {
        anim.SetTrigger("Casting");
        range.gameObject.SetActive(true);
    }

    [RPC]
    public void Cast() {
        particles.Play();
        range.gameObject.SetActive(false);
    }

    public void InstantiateSkill(SkillType skill, Vector3 position, Quaternion rotation, Vector3 targetPosition, ComboModifier modifier) {
        GameObject skillObject = Network.Instantiate(GameManager.instance.skills[(int)skill], position, rotation, 0) as GameObject;
        view.RPC("InitializeSkill", RPCMode.All, skillObject.GetComponent<NetworkView>().viewID, targetPosition, (int)modifier);
    }

    [RPC]
    public void InitializeSkill(NetworkViewID id, Vector3 targetPosition, int modifier) {
        GameObject skillObject = NetworkView.Find(id).gameObject;
        SkillScript skillScript = skillObject.GetComponent<SkillScript>();
        skillScript.Initialize();
        Skill skill = skillScript.skill;
        if (skill != null) {
            skill.targetPosition = targetPosition;
            skill.player = player;
            skill.modifier = (ComboModifier)modifier;
        }
    }
}