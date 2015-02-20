using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NicknameFieldScript : MonoBehaviour {
    public void SetName() {
        Camera.main.GetComponent<GameManager>().playerData.name = GetComponent<Text>().text;
    }
}
