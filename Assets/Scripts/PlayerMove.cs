﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
    private Vector3 destinationPosition;
    private float moveSpeed = 0;
    private ControlScript controlScript;
    private Player player;
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        destinationPosition = transform.position;
        controlScript = GetComponent<ControlScript>();
        player = GetComponent<PlayerScript>().player;
        if (player.owner == Network.player) {
            GetComponent<NetworkView>().RPC("SwitchOwner", RPCMode.All, Network.AllocateViewID());
        }
    }

    [RPC]
    public void SwitchOwner(NetworkViewID newId) {
        GetComponent<NetworkView>().viewID = newId;
    }

    void Update() {
        if (!player.dead) {
            anim.SetFloat("Speed", moveSpeed);
            anim.speed = player.currentSpeed / player.movementSpeed;
            float destinationDistance = Vector3.Distance(destinationPosition, transform.position);
            if (player.currentSpeed > 0.5f) {
                if (destinationDistance < .5f) {
                    moveSpeed = 0;
                } else if (destinationDistance > .5f) {
                    moveSpeed = player.currentSpeed;
                }

                if (controlScript.move) {
                    Plane playerPlane = new Plane(Vector3.up, transform.position);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float hitdist = 0.0f;

                    if (playerPlane.Raycast(ray, out hitdist)) {
                        GetComponent<NetworkView>().RPC("Move", RPCMode.All, ray.GetPoint(hitdist), Quaternion.LookRotation(destinationPosition - transform.position));
                    }
                }
                if (destinationDistance > .5f) {
                    MovePlayer(Vector3.MoveTowards(transform.position, destinationPosition, Time.deltaTime * moveSpeed));
                }
            } else {
                moveSpeed = 0;
                destinationPosition = transform.position;
            }
        }
    }

    public void MovePlayer(Vector3 target) {
        transform.position = Vector3.Lerp(transform.position, target, 1);
    }

    [RPC]
    public void Move(Vector3 destination, Quaternion rotation) {
        destinationPosition = destination;
        transform.rotation = rotation;
        GetComponentInChildren<PlayerStatusScript>().Update();
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            Vector3 destination = destinationPosition;
            stream.Serialize(ref destination);
        } else {
            Vector3 destination = new Vector3();
            stream.Serialize(ref destination);
            destinationPosition = destination;
        }
        GetComponentInChildren<PlayerStatusScript>().Update();
    }
}