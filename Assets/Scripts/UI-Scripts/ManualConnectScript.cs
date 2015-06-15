using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManualConnectScript : MonoBehaviour {
    public void Connect() {
        Network.Connect(GetComponent<Text>().text, GameManager.PORT);
    }
}
