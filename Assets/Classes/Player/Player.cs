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
    public Channel toBeCast { get; set; }
    public bool canCast { get; set; }
    public string name { get; set; }

    private bool positionToBeChanged;
    private Vector3 newPosition;

    public Player() {
        buffs = new LinkedList<Buff>();
        skillSet = new SkillSet();
        maxHealth = 100;
        health = maxHealth;
        positionToBeChanged = false;
        movementSpeed = 6;
        canCast = true;
        name = "";
    }

    public void Update(GameObject gameObject) {
        if (positionToBeChanged) {
            gameObject.transform.position = new Vector3(newPosition.x, gameObject.transform.position.y, newPosition.z);
            positionToBeChanged = false;
        }

        canCast = true;
        currentSpeed = movementSpeed;
        LinkedListNode<Buff> node = buffs.First;
        while (node != null) {
            var nextNode = node.Next;
            node.Value.Update();
            node = nextNode;
        }
        skillSet.Update();

        if (health <= 0)
            Network.Destroy(gameObject);
    }

    public void Damage(float damage) {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    public void Heal(float heal) {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void SchedulePositionChange(Vector3 newPosition) {
        this.newPosition = newPosition;
        positionToBeChanged = true;
    }

    public void AddBuff(Buff buff) {
        buffs.AddLast(buff);
        buff.ApplyBuff();
    }

    public void RemoveBuff(Buff buff) {
        buffs.Remove(buff);
        buff.Unbuff();
    }

    public Channel channel {
        get {
            foreach (Buff b in buffs) {
                if (b.GetType() == typeof(Channel))
                    return b as Channel;
            }
            return null;
        }
    }
}
