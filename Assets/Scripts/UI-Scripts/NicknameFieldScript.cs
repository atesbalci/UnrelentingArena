using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NicknameFieldScript : MonoBehaviour {
    public void SetName() {
        GameManager.instance.playerData.name = GetComponent<Text>().text;
    }
}
