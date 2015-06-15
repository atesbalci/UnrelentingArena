using UnityEngine;
using UnityEngine.Networking;

public class GameNetManager : NetworkLobbyManager {
    public void Connect(string ip, int port) {
        networkAddress = ip;
        networkPort = port;
        StartClient();
    }

    public void Connect(string ip) {
        Connect(ip, networkPort);
    }

    public override void OnStartServer() {
        GameManager.instance.Initialize();
    }

    public override void OnClientConnect(NetworkConnection conn) {
        GameManager.instance.Initialize();
    }

    public void CreateServer() {
        StartHost();
    }

    public void ConnectLocal() {
        Connect("127.0.0.1", networkPort);
    }
}