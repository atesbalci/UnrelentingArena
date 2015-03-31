using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    //movement & health
    public float movementSpeed { get; set; }
    public float currentSpeed { get; set; }
    public float maxHealth { get; set; }
    public float health { get; set; }

    //buffs/skills/items
    public SkillSet skillSet { get; set; }
    public ItemSet itemSet { get; set; }
    public LinkedList<Buff> buffs { get; set; }
    public Channel toBeCast { get; set; }
    public bool canCast { get; set; }

    //game info
    public string name { get; set; }
    public int score { get; set; }
    public Player lastHitter { get; set; }
    public bool dead { get; set; }
    public NetworkPlayer owner { get; set; }

    //blocking
    public float blockingPoints { get; set; }
    public float maxBlockingPoints { get; set; }
    public float blockingExhaust { get; set; }
    public float blockingRegen { get; set; }

    //position change scheduler for skills like blink
    private bool positionToBeChanged;
    private Vector3 newPosition;
    public bool leaveImage { get; set; }

    public Player() {
        buffs = new LinkedList<Buff>();
        skillSet = new SkillSet();
        dead = false;
        maxHealth = 100;
        health = maxHealth;
        positionToBeChanged = false;
        movementSpeed = 3.5f;
        canCast = true;
        name = "";
        score = 0;
        maxBlockingPoints = 1;
        blockingPoints = maxBlockingPoints;
        blockingExhaust = -1;
        blockingRegen = 1;
        leaveImage = false;
    }

    public void Update(GameObject gameObject) {
        if (dead) {
            gameObject.GetComponent<ControlScript>().mine = false;
        }
        if (leaveImage) {
            gameObject.GetComponent<PlayerScript>().LeaveFadingImage();
            leaveImage = false;
        }
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
    }

    public void Damage(float damage, Player hitter) {
        health -= damage;
        if (health < 0)
            health = 0;
        if (hitter != null && this != hitter)
            lastHitter = hitter;
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
        if (buff != null) {
            buffs.AddLast(buff);
            buff.ApplyBuff();
        }
    }

    public void RemoveBuff(Buff buff) {
        if (buff != null) {
            buffs.Remove(buff);
            buff.Unbuff();
        }
    }

    public Channel channel {
        get {
            foreach (Buff b in buffs) {
                if (b is Channel)
                    return b as Channel;
            }
            return null;
        }
    }

    public void Die(GameObject gameObject) {
        gameObject.GetComponent<Collider>().enabled = false;
        dead = true;
        gameObject.GetComponent<ControlScript>().mine = false;
        if (lastHitter != null)
            lastHitter.score += 100;
    }
}
