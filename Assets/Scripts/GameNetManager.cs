using UnityEngine;
using UnityEngine.Networking;

public class GameNetManager : NetworkManager {
    public void Connect(string ip, int port) {
        networkAddress = ip;
        networkPort = port;
        StartClient();
    }

    public override void OnStartServer() {
        GameManager.instance.Initialize();
    }

    public override void OnStopServer() {
        GameManager.instance.Initialize();
    }

    public override void OnClientConnect(NetworkConnection conn) {
        GameManager.instance.Initialize();
    }

    public void CreateServer() {
        StartServer();
    }
}