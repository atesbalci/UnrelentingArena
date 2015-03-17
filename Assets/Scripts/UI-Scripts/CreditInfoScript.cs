using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditInfoScript : MonoBehaviour {
    void Update() {
        GameManager gameManager = Camera.main.GetComponent<GameManager>();
        GetComponentInChildren<Text>().text = "Skill Points: " + gameManager.playerData.skillPoints + 
            "\nCredits: " + gameManager.playerData.credits;
    }
}
