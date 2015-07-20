using UnityEngine;
using System.Collections;

public class PlayerData {
    public string name { get; set; }
    public int score { get; set; }
    public int skillPoints { get; set; }
    public SkillSet skillSet { get; set; }
    public Player currentPlayer { get; set; }

    public PlayerData(string name) {
        this.name = name;
        Clear();
    }

    public void Clear() {
        score = 0;
        skillPoints = 0;
        skillSet = new SkillSet();
        currentPlayer = null;
    }

    public void AddPoints(int points) {
        score += points;
        skillPoints += points;
    }
}
