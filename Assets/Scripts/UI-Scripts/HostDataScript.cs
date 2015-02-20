using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HostDataScript : MonoBehaviour {
    private HostData _hostData;
    public HostData hostData {
        get {
            return _hostData;
        }
        set {
            _hostData = value;
            GetComponentInChildren<Text>().text = hostData.gameName;
        }
    }

    public void OnMouseDown() {
        Network.Connect(hostData);
    }
}
