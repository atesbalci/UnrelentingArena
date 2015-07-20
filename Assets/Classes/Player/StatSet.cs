using UnityEngine;
using System.Collections;

public class StatSet {
    public float maxHealth { get; set; }
    public float movementSpeed { get; set; }
    public float armor { get; set; }
    public float maxEnergyPoints { get; set; }
    public float energyRegen { get; set; }

    public StatSet() {
        maxHealth = 100;
        movementSpeed = 3.5f;
        maxEnergyPoints = 1;
        armor = 10;
        energyRegen = 0.25f;
    }
}
