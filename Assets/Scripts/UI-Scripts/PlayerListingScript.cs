using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerListingScript : MonoBehaviour {
    public Text playerName;
    public Text playerScore;

    public void SetPlayer(PlayerData playerData) {
        playerName.text = playerData.name;
        playerScore.text = "" + playerData.score;
    }
}
