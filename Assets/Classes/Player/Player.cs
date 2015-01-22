using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    private float _movementSpeed;
    public float movementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; currentSpeed = value; } }
    public float currentSpeed { get; set; }
    public float maxHealth { get; set; }
    public float health { get; set; }
    public SkillSet skillSet { get; set; }
    public LinkedList<Buff> buffs { get; set; }
    public bool movementReset { get; set; }

    private bool positionToBeChanged;
    private Vector3 newPosition;

    public Player() {
        buffs = new LinkedList<Buff>();
        skillSet = new SkillSet();
        maxHealth = 100;
        health = maxHealth;
        positionToBeChanged = false;
        movementSpeed = 6;
    }

    public void update(GameObject gameObject) {
        if (positionToBeChanged) {
            gameObject.transform.position = new Vector3(newPosition.x, gameObject.transform.position.y, newPosition.z);
            movementReset = true;
            positionToBeChanged = false;
        }

        LinkedListNode<Buff> node = buffs.First;
        while (node != null) {
            var nextNode = node.Next;
            node.Value.update();
            node = nextNode;
        }
        skillSet.update();

        if (health <= 0)
            Network.Destroy(gameObject);
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

    public void addBuff(Buff buff) {
        buffs.AddLast(buff);
        buff.buff();
    }

    public void removeBuff(Buff buff) {
        buffs.Remove(buff);
        buff.debuff();
    }

    public Channel getChannel() {
        foreach(Buff b in buffs) {
            if (b.GetType() == typeof(Channel))
                return b as Channel;
        }
        return null;
    }
}
