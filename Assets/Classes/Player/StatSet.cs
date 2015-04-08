using UnityEngine;
using System.Collections;

public class StatSet {
    public float maxHealth { get; set; }
    public float movementSpeed { get; set; }
    public float armor { get; set; }
    public float maxBlockingPoints { get; set; }
    public float blockingRegen { get; set; }

    public StatSet() {
        maxHealth = 100;
        movementSpeed = 3.5f;
        maxBlockingPoints = 1;
        armor = 10;
        blockingRegen = 1;
    }
}
