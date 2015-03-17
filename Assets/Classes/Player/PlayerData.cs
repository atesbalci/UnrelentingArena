using UnityEngine;
using System.Collections;

public class PlayerData {
    public string name { get; set; }
    public int score { get; set; }
    public int credits { get; set; }
    public int skillPoints { get; set; }
    public SkillSet skillSet { get; set; }
    public ItemSet itemSet { get; set; }
    public Color color { get; set; }

    public PlayerData(string name) {
        this.name = name;
        Clear();
    }

    public void Clear() {
        credits = 0;
        score = 0;
        skillPoints = 0;
        skillSet = new SkillSet();
        itemSet = new ItemSet();
        color = new Color();
    }

    public void addPoints(int points) {
        score += points;
        credits += points;
        skillPoints += points;
    }
}
