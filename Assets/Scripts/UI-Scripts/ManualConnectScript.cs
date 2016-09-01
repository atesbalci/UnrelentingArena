using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManualConnectScript : MonoBehaviour {
    public void Connect() {
		string text = GetComponent<InputField>().text;
		Network.Connect(text, GameManager.PORT);
    }
}
