using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    public GameObject castAfterEffect;
    public NetworkView view;
    public GameObject chest;

    public bool currentlyCasting { get; set; }

    private Player player;
	private Vector3 targetPoint;
	private int casting;
	private Animator anim;
	private PlayerMove playerMove;
	private MeshRenderer range;
	private LineRenderer rangeLine;
	private SkillPreset castingSkill;
	private ParticleSystem castEffect;

	void Start() {
		player = GetComponent<PlayerScript>().player;
		castEffect = GetComponentInChildren<ParticleSystem>();
		castEffect.startColor = player.color;
		castEffect.Stop();
		casting = -1;
		anim = GetComponent<Animator>();
		playerMove = GetComponent<PlayerMove>();
		rangeLine = GetComponentInChildren<LineRenderer>();
		range = rangeLine.gameObject.GetComponentInChildren<MeshRenderer>();
		range.material.color = new Color(player.color.r, player.color.g, player.color.b, range.material.color.a);
		rangeLine.SetColors(player.color, player.color);
		rangeLine.gameObject.SetActive(false);
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
				castingSkill = player.skillSet.TryToCast(casting);
				if (castingSkill != null) {
					player.casting = true;
					StartCoroutine("CastSkill");
				} else
					casting = -1;
			}

			if (!player.casting)
				CancelCast();
		}
		if(castingSkill != null)
			RefreshRangeLine(castingSkill.range);

		//Vector3 forward = transform.rotation * Vector3.forward;
		//Vector3 left = transform.rotation * Vector3.left;
		//Vector3 right = transform.rotation * Vector3.right;
		//Debug.DrawLine(transform.position, 100 * (Quaternion.RotateTowards(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(left, Vector3.up), 60) * Vector3.forward), Color.red, 0, false);
		//Debug.DrawLine(transform.position, 100 * (Quaternion.RotateTowards(Quaternion.LookRotation(forward, Vector3.up), Quaternion.LookRotation(right, Vector3.up), 60) * Vector3.forward), Color.red, 0, false);
	}

	IEnumerator CastSkill() {
		view.RPC("StartCast", RPCMode.All);
		ComboModifier modifier = player.modifier;
		player.modifier = ComboModifier.Composure;
		currentlyCasting = true;
		targetPoint = new Vector3(0, -100, 0);
		yield return new WaitForSeconds(0.5f);
        while (Input.GetKey(GameInput.instance.keys[(int)GameBinding.Skill1 + castingSkill.key]))
            yield return new WaitForEndOfFrame();
		view.RPC("Cast", RPCMode.All);
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitdist = 0.0f;
		if (playerPlane.Raycast(ray, out hitdist) && view.isMine) {
			targetPoint = ray.GetPoint(hitdist);
			targetPoint = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
		}
		Vector3 forward = transform.rotation * Vector3.forward;
		Vector3 castDir = (targetPoint - transform.position);
		castDir = Vector3.Normalize(new Vector3(castDir.x, 0, castDir.z));
		float castDist = Vector3.Distance(transform.position, targetPoint);
		targetPoint = transform.position + Quaternion.RotateTowards(
            Quaternion.LookRotation(forward, Vector3.up), 
            Quaternion.LookRotation(castDir, Vector3.up), 60) * 
            Vector3.forward * castDist;
		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
		//playerMove.destinationPosition = Vector3.MoveTowards(transform.position, targetPoint, 0.1f);
		InstantiateSkill(castingSkill.skill, new Vector3(transform.position.x, 1, transform.position.z),
			targetRotation, targetPoint, modifier);
		currentlyCasting = false;
		yield return null;
	}

	void LateUpdate() {
		if (anim.GetCurrentAnimatorStateInfo(1).IsName("Cast") && targetPoint != new Vector3(0, -100, 0)) {
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
		anim.SetTrigger("PreCasting");
		rangeLine.gameObject.SetActive(true);
		castEffect.Play();
	}

	[RPC]
	public void Cast() {
        anim.SetTrigger("Cast");
		rangeLine.gameObject.SetActive(false);
		castEffect.Stop();
		LensFlare flare = ((GameObject)Instantiate(castAfterEffect, transform.position, Quaternion.identity)).GetComponent<LensFlare>();
		flare.color = player.color;
	}

	public void InstantiateSkill(SkillType skill, Vector3 position, Quaternion rotation, Vector3 targetPosition, ComboModifier modifier) {
		GameObject skillObject = Network.Instantiate(GameManager.instance.skills[(int)skill], position, rotation, 0) as GameObject;
		view.RPC("InitializeSkill", RPCMode.All, skillObject.GetComponent<NetworkView>().viewID, targetPosition, (int)modifier);
	}

	[RPC]
	public void InitializeSkill(NetworkViewID id, Vector3 targetPosition, int modifier) {
		GameObject skillObject = NetworkView.Find(id).gameObject;
		SkillScript skillScript = skillObject.GetComponent<SkillScript>();
        if (skillScript != null) {
            skillScript.targetPosition = targetPosition;
            skillScript.player = player;
            skillScript.modifier = (ComboModifier)modifier;
		}
	}

	private void RefreshRangeLine(float range) {
		int detail = 10;
		float angle = (60 / detail) * Mathf.Deg2Rad;
		rangeLine.SetPosition(detail, transform.position + range * (transform.rotation * Vector3.forward));
		for (int i = 0; i <= detail; i++) {
			rangeLine.SetPosition(detail + i, transform.position + range * (transform.rotation * (Mathf.Sin(i * angle) * Vector3.right + Mathf.Cos(i * angle) * Vector3.forward)));
			rangeLine.SetPosition(detail - i, transform.position + range * (transform.rotation * (Mathf.Sin(-i * angle) * Vector3.right + Mathf.Cos(-i * angle) * Vector3.forward)));
		}
	}
}