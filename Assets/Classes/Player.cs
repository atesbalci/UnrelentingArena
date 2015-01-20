using UnityEngine;
using System.Collections;

public class Player {
    public float maxHealth { get; set; }
    public float health { get; set; }

    private bool positionToBeChanged;
    private Vector3 newPosition;

    public Player() {
        maxHealth = 100;
        health = maxHealth;
        positionToBeChanged = false;
    }

    public void update(GameObject gameObject, float delta) {
        if (positionToBeChanged) {
            gameObject.transform.position = new Vector3(newPosition.x, gameObject.transform.position.y, newPosition.z);
            gameObject.GetComponent<playerMove>().resetMovement();
            positionToBeChanged = false;
        }
        if (health <= 0)
            MonoBehaviour.Destroy(gameObject);
    }

    public void damage(float damage) {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    public void heal(float heal) {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void schedulePositionChange(Vector3 newPosition) {
        this.newPosition = newPosition;
        positionToBeChanged = true;
    }
}
