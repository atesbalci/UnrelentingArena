using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public StatSet statSet { get; set; }

    //movement & health
    public float currentSpeed { get; set; }
    public float health { get; set; }

    //buffs/skills/items
    public SkillSet skillSet { get; set; }
    private ItemSet _itemSet;
    public ItemSet itemSet { get { return _itemSet; } set { _itemSet = value; itemSet.Apply(this); } }
    public LinkedList<Buff> buffs { get; set; }
    public Channel toBeCast { get; set; }
    public bool canCast { get; set; }

    //game info
    public string name { get; set; }
    public int score { get; set; }
    public Player lastHitter { get; set; }
    public bool dead { get; set; }
    public NetworkPlayer owner { get; set; }
    public GameObject gameObject { get; set; }

    //blocking
    public float blockingPoints { get; set; }
    public float blockingExhaust { get; set; }

    public Color color;

    public Player() {
        buffs = new LinkedList<Buff>();
        skillSet = new SkillSet();
        statSet = new StatSet();
        dead = false;
        health = statSet.maxHealth;
        canCast = true;
        name = "";
        score = 0;
        blockingPoints = statSet.maxBlockingPoints;
        blockingExhaust = -1;
    }

    public void Start(GameObject gameObject) {
        this.gameObject = gameObject;
    }

    public void Update() {
        if (dead) {
            gameObject.GetComponent<ControlScript>().mine = false;
        }

        canCast = true;
        currentSpeed = statSet.movementSpeed;
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
        if (health > statSet.maxHealth)
            health = statSet.maxHealth;
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
