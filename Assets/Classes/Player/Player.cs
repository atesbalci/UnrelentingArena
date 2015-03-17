using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public float movementSpeed { get; set; }
    public float currentSpeed { get; set; }
    public float maxHealth { get; set; }
    public float health { get; set; }
    public SkillSet skillSet { get; set; }
    public ItemSet itemSet { get; set; }
    public LinkedList<Buff> buffs { get; set; }
    public Channel toBeCast { get; set; }
    public bool canCast { get; set; }
    public string name { get; set; }
    public int score { get; set; }
    public Player lastHitter { get; set; }
    public bool dead { get; set; }
    public NetworkPlayer owner { get; set; }

    private bool positionToBeChanged;
    private Vector3 newPosition;

    public Player() {
        buffs = new LinkedList<Buff>();
        skillSet = new SkillSet();
        dead = false;
        maxHealth = 100;
        health = maxHealth;
        positionToBeChanged = false;
        movementSpeed = 4;
        canCast = true;
        name = "";
        score = 0;
    }

    public void Update(GameObject gameObject) {
        if (dead) {
            gameObject.GetComponent<ControlScript>().mine = false;
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
        if (hitter != null)
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
        buffs.AddLast(buff);
        buff.ApplyBuff();
    }

    public void RemoveBuff(Buff buff) {
        buffs.Remove(buff);
        buff.Unbuff();
    }

    public Channel Channel {
        get {
            foreach (Buff b in buffs) {
                if (b.GetType() == typeof(Channel))
                    return b as Channel;
            }
            return null;
        }
    }

    public void Die(GameObject gameObject) {
        gameObject.GetComponent<Collider>().isTrigger = true;
        dead = true;
        gameObject.GetComponent<ControlScript>().mine = false;
        if (lastHitter != null) {
            foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
                if (playerObject.GetComponent<PlayerScript>().player == lastHitter) {
                    playerObject.GetComponent<PlayerScript>().player.score += 100;
                    break;
                }
            }
        }
    }
}
