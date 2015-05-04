using UnityEngine;
using System.Collections;

public class Charge : Skill {
    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        gameObject.transform.SetParent(player.gameObject.transform);
        player.currentSpeed = 0;
    }

    public override void Update() {
        player.gameObject.transform.position = Vector3.MoveTowards(player.gameObject.transform.position, targetPosition, 20 * Time.deltaTime);
        Debug.Log(gameObject.transform.localPosition);
        if (player.gameObject.transform.position == targetPosition) {
            player.currentSpeed = player.statSet.movementSpeed;
            Network.Destroy(gameObject);
        }
    }
}
